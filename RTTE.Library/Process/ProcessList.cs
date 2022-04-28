using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using RTTE.Library.Common;
using RTTE.Library.Common.Interfaces;

namespace RTTE.Library.Process;

/**
 * @TODO: This class needs a serious remake!
 * But for now, I shan't be such a perfectionist.
 **/
public class ProcessList {
    private IWinAPI WinAPI { get; }
    private IPeFileParser PeFileParser { get; }

    private ProcessList(
        IWinAPI winAPI,
        IPeFileParser peFileParser
    ) {
        WinAPI = winAPI;
        PeFileParser = peFileParser;
    }

    public async Task<IEnumerable<Option<IAdvancedProcess>>> AddDeleteUpdate(
        IEnumerable<IProcess> diagnosticProcesses,
        IEnumerable<IAdvancedProcess> advancedProcesses,
        ProcessFilterOptions filterOptions,
        Func<IAdvancedProcess, Task<IAdvancedProcess>> add,
        Func<IAdvancedProcess, int, Task<IAdvancedProcess>> update,
        Func<IAdvancedProcess, Task<IAdvancedProcess>> remove
    ) {
        IEnumerable<Option<IAdvancedProcess>>[] addedOrUpdated = await Utils.ParallelForEachAsync(diagnosticProcesses, 4, async diagProcess => {
            // Why the fuck are these next four lines necessary??? Why can't Option implicitly convert IAdvancedProcess to Option<IAdvancedProcess> ??? 
            IAdvancedProcess results = advancedProcesses.FirstOrDefault(process => process.GetId() == diagProcess.GetId());
            Option<IAdvancedProcess> existing = results != default(IAdvancedProcess) ?
                Option<IAdvancedProcess>.Some(results) :
                Option<IAdvancedProcess>.None;

            return await existing.MatchAsync(
                async existing => {
                    bool matchesFilter = existing.GetMainWindowTitle() != diagProcess.GetMainWindowTitle();

                    async Task<Option<IAdvancedProcess>> tryUpdate(IAdvancedProcess process) {
                        int index = advancedProcesses.ToList().IndexOf(existing);
                        return Option<IAdvancedProcess>.Some(await update(process, index));
                    }

                    return matchesFilter ?
                        await tryUpdate(AdvancedProcess.Create(WinAPI, PeFileParser, diagProcess)) :
                        Option<IAdvancedProcess>.None;
                },
                async () => {
                    bool matchesFilter =
                        $"{diagProcess.GetName()}{diagProcess.GetId()}{diagProcess.GetMainWindowTitle()}".Contains(filterOptions.Filter, StringComparison.InvariantCultureIgnoreCase) &&
                        (!filterOptions.HideWindowless || diagProcess.GetMainWindowHandle() != IntPtr.Zero);

                    async Task<Option<IAdvancedProcess>> tryAdd(IAdvancedProcess advancedProcess) {
                        bool cont = !filterOptions.HideInvalid || advancedProcess.GetArchitecture() == filterOptions.CurrentArchitecture;

                        return cont ?
                            Option<IAdvancedProcess>.Some(await add(advancedProcess)) :
                            Option<IAdvancedProcess>.None;
                    }

                    return matchesFilter ?
                        await tryAdd(AdvancedProcess.Create(WinAPI, PeFileParser, diagProcess)) :
                        Option<IAdvancedProcess>.None;
                }
            );
        });
        IEnumerable<Option<IAdvancedProcess>>[] removed = await Utils.ParallelForEachAsync(advancedProcesses, 4, async advancedProcess => {
            bool doRemove =
                !diagnosticProcesses.Any(diagProcess => advancedProcess.GetId() == diagProcess.GetId()) ||
                !advancedProcess.AsString().Contains(filterOptions.Filter, StringComparison.InvariantCultureIgnoreCase) ||
                (filterOptions.HideInvalid && advancedProcess.GetArchitecture().Match(architecture => architecture != filterOptions.CurrentArchitecture, true)) ||
                (filterOptions.HideWindowless && advancedProcess.GetMainWindowHandle() == IntPtr.Zero);

            return doRemove ?
                Option<IAdvancedProcess>.Some(await remove(advancedProcess)) :
                Option<IAdvancedProcess>.None;
        });

        return addedOrUpdated.Concat(removed).SelectMany(a => a);
    }

    public static ProcessList Create(
        IWinAPI winAPI,
        IPeFileParser parser
    ) => new(
        winAPI,
        parser
    );
}
