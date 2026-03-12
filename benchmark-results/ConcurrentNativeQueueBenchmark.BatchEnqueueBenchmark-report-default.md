# BatchEnqueueBenchmark

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)

Intel Core i7-14700K 3.40GHz, 1 CPU, 28 logical and 20 physical cores

-[Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
-Job-IIOSFQ : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
-Job-QRDOQB : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
-Job-BRFAHL : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
-Job-SRNOFC : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
-Job-RAPTAZ : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
-Job-JMAGVT : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
-Job-EGORRP : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
-Job-MIBEZT : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
-Job-OXFYYB : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3
-Job-VMOMJC : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3

Affinity=0000000000001111111111111111  Concurrent=True  InvocationCount=1  UnrollFactor=1  

 Method                             | Runtime   | Server | Count | Mean       | Error      | StdDev     | Median     | Ratio | RatioSD | Allocated | Alloc Ratio |
----------------------------------- |---------- |------- |------ |-----------:|-----------:|-----------:|-----------:|------:|--------:|----------:|------------:|
 **ConcurrentQueue_EnqueueLoop**        | **.NET 10.0** | **False**  | **1024**  |  **19.487 μs** |  **0.7492 μs** |  **2.0884 μs** |  **18.850 μs** |  **1.01** |    **0.14** |   **33024 B** |        **1.00** |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 10.0 | False  | 1024  |  14.182 μs |  0.5378 μs |  1.5169 μs |  13.950 μs |  0.74 |    0.10 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 10.0 | False  | 1024  |   9.159 μs |  0.5522 μs |  1.5843 μs |   8.900 μs |  0.47 |    0.09 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 10.0 | True   | 1024  |  19.665 μs |  1.2552 μs |  3.4781 μs |  18.400 μs |  1.02 |    0.23 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 10.0 | True   | 1024  |  13.683 μs |  0.7593 μs |  2.1909 μs |  13.100 μs |  0.71 |    0.15 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 10.0 | True   | 1024  |   7.884 μs |  0.6291 μs |  1.8151 μs |   7.400 μs |  0.41 |    0.11 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 6.0  | False  | 1024  |  10.224 μs |  0.2021 μs |  0.2406 μs |  10.100 μs |  1.00 |    0.03 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 6.0  | False  | 1024  |   9.249 μs |  0.1774 μs |  0.4826 μs |   9.200 μs |  0.91 |    0.05 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 6.0  | False  | 1024  |   3.244 μs |  0.1841 μs |  0.5252 μs |   3.200 μs |  0.32 |    0.05 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 6.0  | True   | 1024  |  10.535 μs |  0.2132 μs |  0.5690 μs |  10.300 μs |  1.00 |    0.07 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 6.0  | True   | 1024  |   9.722 μs |  0.2005 μs |  0.5881 μs |   9.700 μs |  0.93 |    0.07 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 6.0  | True   | 1024  |   4.289 μs |  0.2379 μs |  0.7015 μs |   4.300 μs |  0.41 |    0.07 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 7.0  | False  | 1024  |  14.228 μs |  0.3824 μs |  1.1095 μs |  13.700 μs |  1.01 |    0.11 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 7.0  | False  | 1024  |  11.838 μs |  0.4169 μs |  1.2228 μs |  11.500 μs |  0.84 |    0.11 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 7.0  | False  | 1024  |   5.701 μs |  0.4586 μs |  1.3451 μs |   5.400 μs |  0.40 |    0.10 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 7.0  | True   | 1024  |  18.045 μs |  1.5211 μs |  4.4610 μs |  16.300 μs |  1.05 |    0.34 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 7.0  | True   | 1024  |  13.407 μs |  0.4001 μs |  1.1671 μs |  13.300 μs |  0.78 |    0.17 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 7.0  | True   | 1024  |   7.284 μs |  0.4247 μs |  1.2456 μs |   7.000 μs |  0.42 |    0.11 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 8.0  | False  | 1024  |  18.586 μs |  0.6582 μs |  1.8456 μs |  18.000 μs |  1.01 |    0.14 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 8.0  | False  | 1024  |  13.908 μs |  0.5193 μs |  1.4899 μs |  13.500 μs |  0.76 |    0.11 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 8.0  | False  | 1024  |   9.809 μs |  0.5540 μs |  1.6334 μs |   9.600 μs |  0.53 |    0.10 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 8.0  | True   | 1024  |  23.356 μs |  2.1902 μs |  6.3543 μs |  20.800 μs |  1.06 |    0.38 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 8.0  | True   | 1024  |  14.727 μs |  0.4929 μs |  1.4064 μs |  14.500 μs |  0.67 |    0.16 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 8.0  | True   | 1024  |  10.215 μs |  0.6059 μs |  1.7189 μs |   9.800 μs |  0.46 |    0.13 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 9.0  | False  | 1024  |  19.058 μs |  0.6700 μs |  1.8898 μs |  19.000 μs |  1.01 |    0.14 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 9.0  | False  | 1024  |  14.033 μs |  0.5680 μs |  1.6114 μs |  13.600 μs |  0.74 |    0.11 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 9.0  | False  | 1024  |   8.343 μs |  0.5250 μs |  1.4809 μs |   8.100 μs |  0.44 |    0.09 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 9.0  | True   | 1024  |  19.546 μs |  1.2934 μs |  3.4747 μs |  18.300 μs |  1.02 |    0.23 |   33024 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 9.0  | True   | 1024  |  13.324 μs |  0.6540 μs |  1.8554 μs |  12.400 μs |  0.70 |    0.14 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 9.0  | True   | 1024  |   8.205 μs |  0.7331 μs |  2.0915 μs |   7.500 μs |  0.43 |    0.12 |         - |        0.00 |
 **ConcurrentQueue_EnqueueLoop**        | **.NET 10.0** | **False**  | **65536** | **748.750 μs** | **12.9039 μs** | **10.0745 μs** | **746.600 μs** |  **1.00** |    **0.02** | **2098944 B** |        **1.00** |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 10.0 | False  | 65536 | 629.900 μs | 12.0596 μs | 11.2806 μs | 625.700 μs |  0.84 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 10.0 | False  | 65536 | 242.973 μs |  4.4167 μs |  8.2955 μs | 241.500 μs |  0.32 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 10.0 | True   | 65536 | 757.938 μs |  8.1489 μs |  6.8047 μs | 755.900 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 10.0 | True   | 65536 | 641.269 μs | 12.7329 μs | 12.5055 μs | 636.750 μs |  0.85 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 10.0 | True   | 65536 | 231.456 μs |  4.5769 μs |  6.1100 μs | 231.400 μs |  0.31 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 6.0  | False  | 65536 | 778.285 μs |  5.2589 μs |  4.3914 μs | 777.400 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 6.0  | False  | 65536 | 633.350 μs |  7.2833 μs |  6.4565 μs | 632.100 μs |  0.81 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 6.0  | False  | 65536 | 241.056 μs |  4.5837 μs | 11.4150 μs | 238.600 μs |  0.31 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 6.0  | True   | 65536 | 788.356 μs | 14.7476 μs | 15.7797 μs | 782.350 μs |  1.00 |    0.03 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 6.0  | True   | 65536 | 612.080 μs |  4.7895 μs |  4.4801 μs | 611.300 μs |  0.78 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 6.0  | True   | 65536 | 227.042 μs |  4.4486 μs |  4.9446 μs | 225.400 μs |  0.29 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 7.0  | False  | 65536 | 877.171 μs | 17.4620 μs | 15.4796 μs | 875.600 μs |  1.00 |    0.02 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 7.0  | False  | 65536 | 708.154 μs | 14.1141 μs | 11.7859 μs | 707.000 μs |  0.81 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 7.0  | False  | 65536 | 243.212 μs |  4.8445 μs |  7.6839 μs | 242.000 μs |  0.28 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 7.0  | True   | 65536 | 883.900 μs | 15.3074 μs | 12.7823 μs | 885.500 μs |  1.00 |    0.02 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 7.0  | True   | 65536 | 701.536 μs |  5.8413 μs |  5.1782 μs | 700.800 μs |  0.79 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 7.0  | True   | 65536 | 239.995 μs |  4.6758 μs |  5.7424 μs | 239.700 μs |  0.27 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 8.0  | False  | 65536 | 797.492 μs | 15.5760 μs | 20.7935 μs | 790.300 μs |  1.00 |    0.04 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 8.0  | False  | 65536 | 631.182 μs | 12.1351 μs | 19.5960 μs | 622.750 μs |  0.79 |    0.03 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 8.0  | False  | 65536 | 234.894 μs |  4.6999 μs | 12.4634 μs | 231.300 μs |  0.29 |    0.02 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 8.0  | True   | 65536 | 788.100 μs | 15.5964 μs | 24.7376 μs | 779.300 μs |  1.00 |    0.04 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 8.0  | True   | 65536 | 630.200 μs | 12.1933 μs | 10.8090 μs | 624.750 μs |  0.80 |    0.03 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 8.0  | True   | 65536 | 223.238 μs |  4.4410 μs |  4.3616 μs | 222.450 μs |  0.28 |    0.01 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 9.0  | False  | 65536 | 773.379 μs | 14.8395 μs | 28.9433 μs | 761.500 μs |  1.00 |    0.05 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 9.0  | False  | 65536 | 644.984 μs | 12.6810 μs | 14.0948 μs | 642.500 μs |  0.84 |    0.03 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 9.0  | False  | 65536 | 235.693 μs |  4.6686 μs | 10.0497 μs | 235.650 μs |  0.31 |    0.02 |         - |        0.00 |
 ConcurrentQueue_EnqueueLoop        | .NET 9.0  | True   | 65536 | 764.958 μs |  6.6266 μs |  5.1736 μs | 766.050 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue_EnqueueLoop  | .NET 9.0  | True   | 65536 | 626.429 μs | 10.4781 μs | 10.7603 μs | 624.600 μs |  0.82 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_EnqueueRange | .NET 9.0  | True   | 65536 | 238.872 μs |  4.7321 μs |  8.7712 μs | 238.700 μs |  0.31 |    0.01 |         - |        0.00 |
