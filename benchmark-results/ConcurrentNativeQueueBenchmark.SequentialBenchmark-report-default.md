# SequentialBenchmark

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)

Intel Core i7-14700K 3.40GHz, 1 CPU, 28 logical and 20 physical cores

- [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
- Job-IIOSFQ : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
- Job-QRDOQB : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
- Job-BRFAHL : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
- Job-SRNOFC : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
- Job-RAPTAZ : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
- Job-JMAGVT : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
- Job-EGORRP : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
- Job-MIBEZT : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
- Job-OXFYYB : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3
- Job-VMOMJC : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3

Affinity=0000000000001111111111111111  Concurrent=True  InvocationCount=1  UnrollFactor=1  

 Method                      | Runtime   | Server | Count | Mean         | Error      | StdDev     | Median       | Ratio | RatioSD | Allocated | Alloc Ratio |
---------------------------- |---------- |------- |------ |-------------:|-----------:|-----------:|-------------:|------:|--------:|----------:|------------:|
 **ConcurrentQueue**             | **.NET 10.0** | **False**  | **1024**  |    **29.149 μs** |  **0.9572 μs** |  **2.6840 μs** |    **28.300 μs** |  **1.01** |    **0.13** |   **33024 B** |        **1.00** |
 ConcurrentNativeQueue       | .NET 10.0 | False  | 1024  |    14.865 μs |  0.6230 μs |  1.7776 μs |    14.400 μs |  0.51 |    0.08 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 10.0 | False  | 1024  |    15.314 μs |  0.8716 μs |  2.4727 μs |    14.700 μs |  0.53 |    0.10 |         - |        0.00 |
 ConcurrentQueue             | .NET 10.0 | True   | 1024  |    30.855 μs |  2.0105 μs |  5.7035 μs |    28.400 μs |  1.03 |    0.24 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 10.0 | True   | 1024  |    13.593 μs |  0.4649 μs |  1.3265 μs |    13.100 μs |  0.45 |    0.08 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 10.0 | True   | 1024  |    13.969 μs |  0.8534 μs |  2.4486 μs |    13.500 μs |  0.47 |    0.11 |         - |        0.00 |
 ConcurrentQueue             | .NET 6.0  | False  | 1024  |    19.075 μs |  0.3709 μs |  0.2896 μs |    19.100 μs |  1.00 |    0.02 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 6.0  | False  | 1024  |    12.572 μs |  0.2530 μs |  0.6577 μs |    12.500 μs |  0.66 |    0.04 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 6.0  | False  | 1024  |     7.126 μs |  0.3445 μs |  1.0104 μs |     6.800 μs |  0.37 |    0.05 |         - |        0.00 |
 ConcurrentQueue             | .NET 6.0  | True   | 1024  |    19.281 μs |  0.3866 μs |  0.8070 μs |    19.000 μs |  1.00 |    0.06 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 6.0  | True   | 1024  |    13.067 μs |  0.2842 μs |  0.8380 μs |    13.100 μs |  0.68 |    0.05 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 6.0  | True   | 1024  |     8.429 μs |  0.3219 μs |  0.9441 μs |     8.200 μs |  0.44 |    0.05 |         - |        0.00 |
 ConcurrentQueue             | .NET 7.0  | False  | 1024  |    24.094 μs |  0.4758 μs |  0.7683 μs |    23.950 μs |  1.00 |    0.04 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 7.0  | False  | 1024  |    15.370 μs |  0.5301 μs |  1.5548 μs |    14.700 μs |  0.64 |    0.07 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 7.0  | False  | 1024  |     8.832 μs |  0.3370 μs |  0.9777 μs |     8.600 μs |  0.37 |    0.04 |         - |        0.00 |
 ConcurrentQueue             | .NET 7.0  | True   | 1024  |    29.535 μs |  1.9695 μs |  5.7762 μs |    26.500 μs |  1.03 |    0.27 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 7.0  | True   | 1024  |    16.998 μs |  0.4282 μs |  1.2625 μs |    16.900 μs |  0.59 |    0.11 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 7.0  | True   | 1024  |     9.868 μs |  0.3037 μs |  0.8954 μs |     9.700 μs |  0.35 |    0.07 |         - |        0.00 |
 ConcurrentQueue             | .NET 8.0  | False  | 1024  |    29.830 μs |  1.0506 μs |  2.9112 μs |    29.100 μs |  1.01 |    0.13 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 8.0  | False  | 1024  |    15.110 μs |  0.6812 μs |  1.9434 μs |    14.700 μs |  0.51 |    0.08 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 8.0  | False  | 1024  |    15.535 μs |  0.9313 μs |  2.6269 μs |    14.600 μs |  0.53 |    0.10 |         - |        0.00 |
 ConcurrentQueue             | .NET 8.0  | True   | 1024  |    37.570 μs |  2.4687 μs |  7.2403 μs |    37.700 μs |  1.04 |    0.28 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 8.0  | True   | 1024  |    15.931 μs |  0.4358 μs |  1.2149 μs |    15.900 μs |  0.44 |    0.09 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 8.0  | True   | 1024  |    16.450 μs |  0.9512 μs |  2.6830 μs |    15.950 μs |  0.45 |    0.11 |         - |        0.00 |
 ConcurrentQueue             | .NET 9.0  | False  | 1024  |    29.950 μs |  0.9020 μs |  2.5441 μs |    29.500 μs |  1.01 |    0.12 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 9.0  | False  | 1024  |    14.454 μs |  0.4090 μs |  1.1469 μs |    14.300 μs |  0.49 |    0.05 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 9.0  | False  | 1024  |    15.590 μs |  1.0241 μs |  2.8886 μs |    15.200 μs |  0.52 |    0.11 |         - |        0.00 |
 ConcurrentQueue             | .NET 9.0  | True   | 1024  |    29.657 μs |  0.9650 μs |  2.7532 μs |    28.700 μs |  1.01 |    0.13 |   33024 B |        1.00 |
 ConcurrentNativeQueue       | .NET 9.0  | True   | 1024  |    14.041 μs |  0.6953 μs |  1.9950 μs |    13.100 μs |  0.48 |    0.08 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 9.0  | True   | 1024  |    14.422 μs |  0.9533 μs |  2.6574 μs |    13.850 μs |  0.49 |    0.10 |         - |        0.00 |
 **ConcurrentQueue**             | **.NET 10.0** | **False**  | **65536** | **1,347.218 μs** | **26.7471 μs** | **27.4673 μs** | **1,342.300 μs** |  **1.00** |    **0.03** | **2098944 B** |        **1.00** |
 ConcurrentNativeQueue       | .NET 10.0 | False  | 65536 |   748.053 μs | 14.8753 μs | 15.2759 μs |   743.100 μs |  0.56 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 10.0 | False  | 65536 |   726.532 μs | 13.9162 μs | 15.4679 μs |   727.000 μs |  0.54 |    0.02 |         - |        0.00 |
 ConcurrentQueue             | .NET 10.0 | True   | 65536 | 1,321.867 μs | 16.3293 μs | 12.7489 μs | 1,321.600 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 10.0 | True   | 65536 |   725.975 μs | 13.7393 μs | 13.4939 μs |   723.000 μs |  0.55 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 10.0 | True   | 65536 |   729.386 μs | 14.5839 μs | 24.7645 μs |   721.700 μs |  0.55 |    0.02 |         - |        0.00 |
 ConcurrentQueue             | .NET 6.0  | False  | 65536 | 1,326.208 μs | 13.0209 μs | 10.8730 μs | 1,328.500 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 6.0  | False  | 65536 |   892.969 μs | 17.4960 μs | 25.6454 μs |   883.500 μs |  0.67 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 6.0  | False  | 65536 |   523.983 μs |  9.0770 μs | 13.3049 μs |   522.100 μs |  0.40 |    0.01 |         - |        0.00 |
 ConcurrentQueue             | .NET 6.0  | True   | 65536 | 1,325.785 μs | 13.2936 μs | 11.1007 μs | 1,323.800 μs |  1.00 |    0.01 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 6.0  | True   | 65536 |   829.387 μs |  9.7554 μs |  9.1252 μs |   829.500 μs |  0.63 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 6.0  | True   | 65536 |   486.072 μs |  9.0860 μs | 13.3181 μs |   481.300 μs |  0.37 |    0.01 |         - |        0.00 |
 ConcurrentQueue             | .NET 7.0  | False  | 65536 | 1,489.560 μs | 26.7674 μs | 25.0382 μs | 1,482.900 μs |  1.00 |    0.02 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 7.0  | False  | 65536 |   941.335 μs | 18.2346 μs | 18.7255 μs |   936.900 μs |  0.63 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 7.0  | False  | 65536 |   583.738 μs | 11.5530 μs | 27.2319 μs |   581.950 μs |  0.39 |    0.02 |         - |        0.00 |
 ConcurrentQueue             | .NET 7.0  | True   | 65536 | 1,472.150 μs | 25.8088 μs | 22.8789 μs | 1,461.100 μs |  1.00 |    0.02 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 7.0  | True   | 65536 |   931.700 μs | 18.0345 μs | 26.4348 μs |   928.200 μs |  0.63 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 7.0  | True   | 65536 |   554.271 μs | 10.8676 μs | 17.5491 μs |   552.950 μs |  0.38 |    0.01 |         - |        0.00 |
 ConcurrentQueue             | .NET 8.0  | False  | 65536 | 1,397.282 μs | 24.2491 μs | 39.1578 μs | 1,383.200 μs |  1.00 |    0.04 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 8.0  | False  | 65536 |   911.991 μs | 18.0284 μs | 22.8002 μs |   911.100 μs |  0.65 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 8.0  | False  | 65536 |   729.955 μs | 14.3891 μs | 22.8226 μs |   730.900 μs |  0.52 |    0.02 |         - |        0.00 |
 ConcurrentQueue             | .NET 8.0  | True   | 65536 | 1,384.993 μs | 22.7144 μs | 21.2471 μs | 1,377.900 μs |  1.00 |    0.02 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 8.0  | True   | 65536 |   882.700 μs | 15.9313 μs | 17.7076 μs |   881.300 μs |  0.64 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 8.0  | True   | 65536 |   705.257 μs | 13.5469 μs | 17.1326 μs |   702.100 μs |  0.51 |    0.01 |         - |        0.00 |
 ConcurrentQueue             | .NET 9.0  | False  | 65536 | 1,396.839 μs | 27.5650 μs | 46.0549 μs | 1,382.750 μs |  1.00 |    0.05 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 9.0  | False  | 65536 |   755.297 μs | 15.0548 μs | 24.3107 μs |   749.500 μs |  0.54 |    0.02 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 9.0  | False  | 65536 |   745.486 μs | 14.6074 μs | 21.4113 μs |   748.500 μs |  0.53 |    0.02 |         - |        0.00 |
 ConcurrentQueue             | .NET 9.0  | True   | 65536 | 1,374.262 μs | 26.8845 μs | 32.0041 μs | 1,362.000 μs |  1.00 |    0.03 | 2098944 B |        1.00 |
 ConcurrentNativeQueue       | .NET 9.0  | True   | 65536 |   745.608 μs | 13.9982 μs | 11.6891 μs |   744.900 μs |  0.54 |    0.01 |         - |        0.00 |
 ConcurrentNativeQueue_Batch | .NET 9.0  | True   | 65536 |   763.098 μs | 15.1899 μs | 30.6844 μs |   754.550 μs |  0.56 |    0.03 |         - |        0.00 |
