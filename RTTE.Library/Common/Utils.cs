using LanguageExt;
using RTTE.Library.Common.Interfaces;
using RTTE.Library.Native;
using RTTE.Library.Process;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTTE.Library.Common;

public static class Utils {
    private static Option<IntPtr> OpenProcess(IWinAPI winAPI, int processId, bool inheritHandle, params uint[] accessRights) {
        static IntPtr continueOpenProcess(IWinAPI winAPI, int processId, bool inheritHandle, params uint[] accessRights) {
            uint accessRightsFlag = accessRights.First();
            IEnumerable<uint> tail = accessRights.Skip(1);

            foreach (uint accessRight in tail)
                accessRightsFlag |= accessRight;

            return winAPI.OpenProcess(accessRightsFlag, inheritHandle, (uint)processId);
        }

        return accessRights.Any() ?
            continueOpenProcess(winAPI, processId, inheritHandle, accessRights) :
            Option<IntPtr>.None;
    }

    public static Option<IntPtr> GetHandleSafe(IWinAPI winAPI, int processId)
        => OpenProcess(
            winAPI,
            processId,
            false,
            ProcessAccessRight.AllAccess,
            ProcessAccessRight.VmOperation,
            ProcessAccessRight.VmWrite
        );

    public static Task ParallelForEachAsync<T>(
        IEnumerable<T> source,
        int degreesOfParallelism,
        Func<T, Task> body
    ) {
        async Task AwaitPartition(IEnumerator<T> partition) {
            using (partition) {
                while (partition.MoveNext())
                    await body(partition.Current);
            }
        }

        return Task.WhenAll(
            Partitioner
                .Create(source)
                .GetPartitions(degreesOfParallelism)
                .AsParallel()
                .Select(AwaitPartition)
        );
    }

    public static Task<IEnumerable<T2>[]> ParallelForEachAsync<T, T2>(
        IEnumerable<T> source,
        int degreesOfParallelism,
        Func<T, Task<T2>> body
    ) {
        async Task<IEnumerable<T2>> AwaitPartition(IEnumerator<T> partition) {
            List<T2> objects = new();
            using (partition) {
                while (partition.MoveNext())
                    objects.Add(await body(partition.Current));
            }

            return objects;
        }

        return Task.WhenAll(
            Partitioner
                .Create(source)
                .GetPartitions(degreesOfParallelism)
                .AsParallel()
                .Select(AwaitPartition)
        );
    }

    public static ProcessArchitecture GetCurrentArchitecture()
        => Environment.Is64BitProcess ? ProcessArchitecture.x64 : ProcessArchitecture.x86;

    public static Func<T1, T2, T3, Option<TResult>> DisregardException<TException, T1, T2, T3, TResult>(
        Func<T1, T2, T3, TResult> func
    ) where TException : Exception
        => new((arg1, arg2, arg3) => {
            try {
                return func(arg1, arg2, arg3);
            } catch (TException) {
                    // disregard
                    return Option<TResult>.None;
            }
        });

    public static Func<T1, T2, T3, T4, Option<TResult>> DisregardException<TException, T1, T2, T3, T4, TResult>(
        Func<T1, T2, T3, T4, TResult> func
    ) where TException : Exception
        => new((arg1, arg2, arg3, arg4) => {
            try {
                return func(arg1, arg2, arg3, arg4);
            } catch (TException) {
                    // disregard
                    return Option<TResult>.None;
            }
        });
}
