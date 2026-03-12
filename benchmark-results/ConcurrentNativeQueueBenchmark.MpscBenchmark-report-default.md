
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
Intel Core i7-14700K 3.40GHz, 1 CPU, 28 logical and 20 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-IIOSFQ : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-QRDOQB : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-BRFAHL : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
  Job-SRNOFC : .NET 6.0.36 (6.0.36, 6.0.3624.51421), X64 RyuJIT x86-64-v3
  Job-RAPTAZ : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
  Job-JMAGVT : .NET 7.0.20 (7.0.20, 7.0.2024.26716), X64 RyuJIT x86-64-v3
  Job-EGORRP : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  Job-MIBEZT : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  Job-OXFYYB : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3
  Job-VMOMJC : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3

Affinity=0000000000001111111111111111  Concurrent=True  InvocationCount=1  
UnrollFactor=1  

 Method                | Runtime   | Server | ProducerCount | Mean       | Error     | StdDev     | Median     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated | Alloc Ratio |
---------------------- |---------- |------- |-------------- |-----------:|----------:|-----------:|-----------:|------:|--------:|----------:|----------:|----------:|----------:|------------:|
 **ConcurrentQueue**       | **.NET 10.0** | **False**  | **1**             |  **10.018 ms** | **0.3098 ms** |  **0.9085 ms** |   **9.815 ms** |  **1.01** |    **0.13** |         **-** |         **-** |         **-** | **2099432 B** |       **1.000** |
 ConcurrentNativeQueue | .NET 10.0 | False  | 1             |  25.847 ms | 0.5104 ms |  0.8667 ms |  25.703 ms |  2.60 |    0.25 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 10.0 | False  | 1             |  88.935 ms | 1.7655 ms |  3.9489 ms |  89.150 ms |  8.95 |    0.89 |         - |         - |         - |   68648 B |       0.033 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 10.0 | True   | 1             |   9.823 ms | 0.3031 ms |  0.8842 ms |   9.722 ms |  1.01 |    0.13 |         - |         - |         - | 2099432 B |       1.000 |
 ConcurrentNativeQueue | .NET 10.0 | True   | 1             |  25.570 ms | 0.5098 ms |  0.9943 ms |  25.657 ms |  2.62 |    0.25 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 10.0 | True   | 1             |  92.012 ms | 1.8350 ms |  4.8019 ms |  91.142 ms |  9.44 |    0.96 |         - |         - |         - |  134632 B |       0.064 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | False  | 1             |  10.703 ms | 0.5041 ms |  1.4625 ms |  10.726 ms |  1.02 |    0.20 |         - |         - |         - |  263656 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | False  | 1             |  18.124 ms | 0.3333 ms |  0.3117 ms |  18.138 ms |  1.72 |    0.24 |         - |         - |         - |     488 B |       0.002 |
 UnboundedChannel      | .NET 6.0  | False  | 1             |  97.522 ms | 1.9369 ms |  4.7147 ms |  97.112 ms |  9.28 |    1.34 |         - |         - |         - |  134632 B |       0.511 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | True   | 1             |  10.610 ms | 0.5990 ms |  1.7660 ms |  10.640 ms |  1.03 |    0.24 |         - |         - |         - |   33512 B |        1.00 |
 ConcurrentNativeQueue | .NET 6.0  | True   | 1             |  18.223 ms | 0.3567 ms |  0.4380 ms |  18.288 ms |  1.76 |    0.29 |         - |         - |         - |     488 B |        0.01 |
 UnboundedChannel      | .NET 6.0  | True   | 1             |  98.828 ms | 2.0566 ms |  5.8676 ms |  98.163 ms |  9.57 |    1.66 |         - |         - |         - |  134632 B |        4.02 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | False  | 1             |   9.608 ms | 0.4988 ms |  1.3988 ms |   9.443 ms |  1.02 |    0.20 |         - |         - |         - |  526056 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | False  | 1             |  27.115 ms | 0.9759 ms |  2.8775 ms |  27.321 ms |  2.88 |    0.50 |         - |         - |         - |     488 B |       0.001 |
 UnboundedChannel      | .NET 7.0  | False  | 1             | 101.727 ms | 2.6139 ms |  7.7071 ms |  99.722 ms | 10.80 |    1.69 |         - |         - |         - |   68648 B |       0.130 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | True   | 1             |   8.945 ms | 0.1986 ms |  0.5855 ms |   8.881 ms |  1.00 |    0.09 |         - |         - |         - | 1050600 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | True   | 1             |  18.748 ms | 0.3661 ms |  0.3596 ms |  18.749 ms |  2.10 |    0.14 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 7.0  | True   | 1             |  96.456 ms | 1.9072 ms |  3.6286 ms |  96.358 ms | 10.83 |    0.80 |         - |         - |         - |   68648 B |       0.065 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | False  | 1             |   9.556 ms | 0.2165 ms |  0.6211 ms |   9.452 ms |  1.00 |    0.09 |         - |         - |         - | 1050600 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | False  | 1             |  25.498 ms | 0.4910 ms |  0.5254 ms |  25.508 ms |  2.68 |    0.18 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 8.0  | False  | 1             |  95.217 ms | 1.8970 ms |  4.4341 ms |  96.131 ms | 10.00 |    0.79 |         - |         - |         - |  134632 B |       0.128 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | True   | 1             |  10.324 ms | 0.2852 ms |  0.8366 ms |  10.403 ms |  1.01 |    0.12 |         - |         - |         - |  526056 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | True   | 1             |  17.941 ms | 0.3491 ms |  0.4020 ms |  18.002 ms |  1.75 |    0.15 |         - |         - |         - |     488 B |       0.001 |
 UnboundedChannel      | .NET 8.0  | True   | 1             |  99.344 ms | 1.9866 ms |  4.8357 ms |  99.617 ms |  9.69 |    0.94 |         - |         - |         - |   68648 B |       0.130 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | False  | 1             |   9.658 ms | 0.2097 ms |  0.6084 ms |   9.663 ms |  1.00 |    0.09 |         - |         - |         - | 2099432 B |       1.000 |
 ConcurrentNativeQueue | .NET 9.0  | False  | 1             |  17.380 ms | 0.3365 ms |  0.3305 ms |  17.419 ms |  1.81 |    0.12 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 9.0  | False  | 1             |  99.165 ms | 1.9552 ms |  2.6101 ms |  99.118 ms | 10.31 |    0.69 |         - |         - |         - |  528744 B |       0.252 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | True   | 1             |   9.722 ms | 0.2221 ms |  0.6478 ms |   9.587 ms |  1.00 |    0.09 |         - |         - |         - | 2099432 B |       1.000 |
 ConcurrentNativeQueue | .NET 9.0  | True   | 1             |  17.308 ms | 0.2754 ms |  0.2576 ms |  17.300 ms |  1.79 |    0.12 |         - |         - |         - |     488 B |       0.000 |
 UnboundedChannel      | .NET 9.0  | True   | 1             |  95.068 ms | 1.9295 ms |  5.5049 ms |  94.899 ms |  9.82 |    0.86 |         - |         - |         - |   68648 B |       0.033 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 **ConcurrentQueue**       | **.NET 10.0** | **False**  | **2**             |  **72.049 ms** | **2.9831 ms** |  **8.7957 ms** |  **73.246 ms** |  **1.02** |    **0.18** |         **-** |         **-** |         **-** |  **263912 B** |       **1.000** |
 ConcurrentNativeQueue | .NET 10.0 | False  | 2             |  42.230 ms | 0.8337 ms |  0.7799 ms |  42.353 ms |  0.60 |    0.08 |         - |         - |         - |     744 B |       0.003 |
 UnboundedChannel      | .NET 10.0 | False  | 2             |  80.203 ms | 1.5970 ms |  4.7088 ms |  79.307 ms |  1.13 |    0.17 |         - |         - |         - |   35688 B |       0.135 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 10.0 | True   | 2             |  69.879 ms | 3.2338 ms |  9.5349 ms |  70.745 ms |  1.02 |    0.20 |         - |         - |         - |  526312 B |       1.000 |
 ConcurrentNativeQueue | .NET 10.0 | True   | 2             |  42.660 ms | 0.8069 ms |  1.0204 ms |  42.707 ms |  0.62 |    0.09 |         - |         - |         - |     744 B |       0.001 |
 UnboundedChannel      | .NET 10.0 | True   | 2             |  80.003 ms | 1.5920 ms |  4.3040 ms |  79.356 ms |  1.17 |    0.18 |         - |         - |         - |   35688 B |       0.068 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | False  | 2             |  68.815 ms | 3.0340 ms |  8.9459 ms |  70.330 ms |  1.02 |    0.20 |         - |         - |         - | 1050856 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | False  | 2             |  44.303 ms | 0.8812 ms |  1.4478 ms |  44.128 ms |  0.66 |    0.09 |         - |         - |         - |     744 B |       0.001 |
 UnboundedChannel      | .NET 6.0  | False  | 2             |  81.898 ms | 1.7891 ms |  5.2753 ms |  81.509 ms |  1.21 |    0.19 |         - |         - |         - | 2102760 B |       2.001 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | True   | 2             |  71.745 ms | 3.2726 ms |  9.6494 ms |  73.389 ms |  1.02 |    0.21 |         - |         - |         - | 4197096 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | True   | 2             |  45.721 ms | 0.9064 ms |  2.1542 ms |  45.405 ms |  0.65 |    0.11 |         - |         - |         - |     744 B |       0.000 |
 UnboundedChannel      | .NET 6.0  | True   | 2             |  83.279 ms | 1.9556 ms |  5.7661 ms |  83.624 ms |  1.19 |    0.20 |         - |         - |         - |   68904 B |       0.016 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | False  | 2             |  68.246 ms | 3.0256 ms |  8.9210 ms |  69.360 ms |  1.02 |    0.20 |         - |         - |         - |  132584 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | False  | 2             |  43.300 ms | 0.8596 ms |  1.0233 ms |  43.452 ms |  0.65 |    0.09 |         - |         - |         - |     744 B |       0.006 |
 UnboundedChannel      | .NET 7.0  | False  | 2             |  82.183 ms | 1.8408 ms |  5.4278 ms |  82.344 ms |  1.23 |    0.19 |         - |         - |         - |  134888 B |       1.017 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | True   | 2             |  70.343 ms | 3.5973 ms | 10.6066 ms |  73.378 ms |  1.03 |    0.24 |         - |         - |         - |  132584 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | True   | 2             |  46.700 ms | 0.9289 ms |  1.9593 ms |  46.889 ms |  0.68 |    0.12 |         - |         - |         - |     744 B |       0.006 |
 UnboundedChannel      | .NET 7.0  | True   | 2             |  84.636 ms | 1.8835 ms |  5.5535 ms |  85.901 ms |  1.23 |    0.23 |         - |         - |         - |   35688 B |       0.269 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | False  | 2             |  71.370 ms | 2.9992 ms |  8.8433 ms |  72.796 ms |  1.02 |    0.19 |         - |         - |         - |  526312 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | False  | 2             |  41.148 ms | 0.8093 ms |  0.7949 ms |  41.393 ms |  0.59 |    0.08 |         - |         - |         - |     744 B |       0.001 |
 UnboundedChannel      | .NET 8.0  | False  | 2             |  81.448 ms | 1.6190 ms |  4.4593 ms |  81.969 ms |  1.16 |    0.18 |         - |         - |         - |   35688 B |       0.068 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | True   | 2             |  70.543 ms | 3.3904 ms |  9.9968 ms |  72.415 ms |  1.02 |    0.23 |         - |         - |         - |  263912 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | True   | 2             |  43.018 ms | 0.8459 ms |  1.3417 ms |  43.151 ms |  0.62 |    0.11 |         - |         - |         - |     744 B |       0.003 |
 UnboundedChannel      | .NET 8.0  | True   | 2             |  81.066 ms | 1.8049 ms |  5.3219 ms |  82.118 ms |  1.18 |    0.22 |         - |         - |         - |   35688 B |       0.135 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | False  | 2             |  69.077 ms | 2.6145 ms |  7.7088 ms |  70.184 ms |  1.01 |    0.17 |         - |         - |         - |  526312 B |       1.000 |
 ConcurrentNativeQueue | .NET 9.0  | False  | 2             |  41.254 ms | 0.8160 ms |  1.0894 ms |  40.974 ms |  0.61 |    0.07 |         - |         - |         - |     744 B |       0.001 |
 UnboundedChannel      | .NET 9.0  | False  | 2             |  80.776 ms | 1.6083 ms |  3.4963 ms |  81.568 ms |  1.19 |    0.15 |         - |         - |         - |   18856 B |       0.036 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | True   | 2             |  69.398 ms | 3.3190 ms |  9.7860 ms |  70.602 ms |  1.02 |    0.23 |         - |         - |         - |  132584 B |       1.000 |
 ConcurrentNativeQueue | .NET 9.0  | True   | 2             |  41.244 ms | 0.8063 ms |  1.3021 ms |  41.272 ms |  0.61 |    0.11 |         - |         - |         - |     744 B |       0.006 |
 UnboundedChannel      | .NET 9.0  | True   | 2             |  81.664 ms | 1.6297 ms |  4.5431 ms |  82.319 ms |  1.20 |    0.22 |         - |         - |         - |   68904 B |       0.520 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 **ConcurrentQueue**       | **.NET 10.0** | **False**  | **4**             | **114.191 ms** | **2.2632 ms** |  **6.0800 ms** | **113.570 ms** |  **1.00** |    **0.08** |         **-** |         **-** |         **-** |   **34232 B** |        **1.00** |
 ConcurrentNativeQueue | .NET 10.0 | False  | 4             |  44.542 ms | 0.4848 ms |  0.4049 ms |  44.574 ms |  0.39 |    0.02 |         - |         - |         - |    1208 B |        0.04 |
 UnboundedChannel      | .NET 10.0 | False  | 4             |  83.531 ms | 1.6521 ms |  3.3748 ms |  84.190 ms |  0.73 |    0.05 |         - |         - |         - |   36152 B |        1.06 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 10.0 | True   | 4             | 112.936 ms | 2.2491 ms |  5.8856 ms | 112.472 ms |  1.00 |    0.07 |         - |         - |         - |   67256 B |        1.00 |
 ConcurrentNativeQueue | .NET 10.0 | True   | 4             |  44.876 ms | 0.5369 ms |  0.5022 ms |  44.641 ms |  0.40 |    0.02 |         - |         - |         - |    1208 B |        0.02 |
 UnboundedChannel      | .NET 10.0 | True   | 4             |  83.134 ms | 1.6591 ms |  3.4263 ms |  83.783 ms |  0.74 |    0.05 |         - |         - |         - |   69368 B |        1.03 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | False  | 4             | 114.226 ms | 2.2358 ms |  3.4808 ms | 113.923 ms |  1.00 |    0.04 | 1000.0000 | 1000.0000 | 1000.0000 | 8392456 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | False  | 4             |  52.533 ms | 1.0549 ms |  3.1103 ms |  52.946 ms |  0.46 |    0.03 |         - |         - |         - |    1208 B |       0.000 |
 UnboundedChannel      | .NET 6.0  | False  | 4             |  86.531 ms | 1.7274 ms |  3.0705 ms |  86.560 ms |  0.76 |    0.04 |         - |         - |         - |  529464 B |       0.063 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | True   | 4             | 113.341 ms | 2.2643 ms |  6.1216 ms | 113.896 ms |  1.00 |    0.08 |         - |         - |         - |  526776 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | True   | 4             |  54.001 ms | 1.0800 ms |  3.0987 ms |  54.214 ms |  0.48 |    0.04 |         - |         - |         - |    1208 B |       0.002 |
 UnboundedChannel      | .NET 6.0  | True   | 4             |  87.990 ms | 1.7439 ms |  3.4014 ms |  89.140 ms |  0.78 |    0.05 |         - |         - |         - |   69368 B |       0.132 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | False  | 4             | 115.491 ms | 2.2972 ms |  5.4148 ms | 114.874 ms |  1.00 |    0.07 |         - |         - |         - |  133048 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | False  | 4             |  46.687 ms | 0.6665 ms |  0.6235 ms |  46.675 ms |  0.41 |    0.02 |         - |         - |         - |    1208 B |       0.009 |
 UnboundedChannel      | .NET 7.0  | False  | 4             |  89.518 ms | 1.7745 ms |  3.1079 ms |  89.464 ms |  0.78 |    0.05 |         - |         - |         - |   36152 B |       0.272 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | True   | 4             | 111.898 ms | 2.2230 ms |  5.7778 ms | 111.752 ms |  1.00 |    0.07 |         - |         - |         - |   34232 B |        1.00 |
 ConcurrentNativeQueue | .NET 7.0  | True   | 4             |  53.633 ms | 1.0577 ms |  2.3657 ms |  53.809 ms |  0.48 |    0.03 |         - |         - |         - |    1208 B |        0.04 |
 UnboundedChannel      | .NET 7.0  | True   | 4             |  88.027 ms | 1.7145 ms |  4.0413 ms |  88.058 ms |  0.79 |    0.05 |         - |         - |         - |   36152 B |        1.06 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | False  | 4             | 112.409 ms | 2.2174 ms |  4.4792 ms | 113.017 ms |  1.00 |    0.06 |         - |         - |         - |  133048 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | False  | 4             |  44.577 ms | 0.8706 ms |  1.0026 ms |  44.563 ms |  0.40 |    0.02 |         - |         - |         - |    1208 B |       0.009 |
 UnboundedChannel      | .NET 8.0  | False  | 4             |  84.852 ms | 1.6471 ms |  2.3623 ms |  85.363 ms |  0.76 |    0.04 |         - |         - |         - |   36152 B |       0.272 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | True   | 4             | 114.460 ms | 2.2704 ms |  6.4036 ms | 115.294 ms |  1.00 |    0.08 |         - |         - |         - |   67256 B |        1.00 |
 ConcurrentNativeQueue | .NET 8.0  | True   | 4             |  45.562 ms | 0.7418 ms |  0.6576 ms |  45.727 ms |  0.40 |    0.02 |         - |         - |         - |    1208 B |        0.02 |
 UnboundedChannel      | .NET 8.0  | True   | 4             |  86.644 ms | 1.7097 ms |  4.0633 ms |  86.921 ms |  0.76 |    0.06 |         - |         - |         - |   36152 B |        0.54 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | False  | 4             | 113.879 ms | 2.2320 ms |  4.0248 ms | 113.991 ms |  1.00 |    0.05 |         - |         - |         - |   34232 B |        1.00 |
 ConcurrentNativeQueue | .NET 9.0  | False  | 4             |  44.933 ms | 0.8777 ms |  0.9756 ms |  45.094 ms |  0.40 |    0.02 |         - |         - |         - |    1208 B |        0.04 |
 UnboundedChannel      | .NET 9.0  | False  | 4             |  85.062 ms | 1.6028 ms |  3.1262 ms |  85.491 ms |  0.75 |    0.04 |         - |         - |         - |  266872 B |        7.80 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | True   | 4             | 115.979 ms | 2.2942 ms |  5.4968 ms | 114.846 ms |  1.00 |    0.07 |         - |         - |         - |   67256 B |        1.00 |
 ConcurrentNativeQueue | .NET 9.0  | True   | 4             |  44.434 ms | 0.6789 ms |  0.6018 ms |  44.390 ms |  0.38 |    0.02 |         - |         - |         - |    1208 B |        0.02 |
 UnboundedChannel      | .NET 9.0  | True   | 4             |  86.548 ms | 1.7263 ms |  3.4477 ms |  87.159 ms |  0.75 |    0.05 |         - |         - |         - |   69368 B |        1.03 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 **ConcurrentQueue**       | **.NET 10.0** | **False**  | **8**             | **134.273 ms** | **2.6847 ms** |  **4.1797 ms** | **134.674 ms** |  **1.00** |    **0.04** |         **-** |         **-** |         **-** |   **35160 B** |        **1.00** |
 ConcurrentNativeQueue | .NET 10.0 | False  | 8             |  46.354 ms | 0.8823 ms |  2.2931 ms |  46.135 ms |  0.35 |    0.02 |         - |         - |         - |    2136 B |        0.06 |
 UnboundedChannel      | .NET 10.0 | False  | 8             |  84.447 ms | 1.6877 ms |  3.8094 ms |  85.517 ms |  0.63 |    0.03 |         - |         - |         - |   70296 B |        2.00 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 10.0 | True   | 8             | 134.040 ms | 2.4123 ms |  3.7556 ms | 134.490 ms |  1.00 |    0.04 |         - |         - |         - |  265304 B |       1.000 |
 ConcurrentNativeQueue | .NET 10.0 | True   | 8             |  45.859 ms | 0.9167 ms |  2.5401 ms |  45.775 ms |  0.34 |    0.02 |         - |         - |         - |    2136 B |       0.008 |
 UnboundedChannel      | .NET 10.0 | True   | 8             |  84.596 ms | 1.6570 ms |  3.2707 ms |  84.808 ms |  0.63 |    0.03 |         - |         - |         - |   70296 B |       0.265 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | False  | 8             | 137.343 ms | 2.7336 ms |  6.1701 ms | 136.976 ms |  1.00 |    0.06 |         - |         - |         - | 4198488 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | False  | 8             |  52.957 ms | 1.0559 ms |  2.9436 ms |  52.959 ms |  0.39 |    0.03 |         - |         - |         - |    2136 B |       0.001 |
 UnboundedChannel      | .NET 6.0  | False  | 8             |  87.129 ms | 1.7282 ms |  3.6829 ms |  87.263 ms |  0.64 |    0.04 |         - |         - |         - |  530392 B |       0.126 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 6.0  | True   | 8             | 137.990 ms | 2.7504 ms |  4.8171 ms | 138.554 ms |  1.00 |    0.05 |         - |         - |         - |  265304 B |       1.000 |
 ConcurrentNativeQueue | .NET 6.0  | True   | 8             |  51.801 ms | 0.9879 ms |  0.7713 ms |  51.976 ms |  0.38 |    0.01 |         - |         - |         - |    2136 B |       0.008 |
 UnboundedChannel      | .NET 6.0  | True   | 8             |  86.447 ms | 1.6910 ms |  2.7783 ms |  86.588 ms |  0.63 |    0.03 |         - |         - |         - |  530392 B |       1.999 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | False  | 8             | 137.964 ms | 2.8409 ms |  8.3763 ms | 136.114 ms |  1.00 |    0.08 |         - |         - |         - | 4198488 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | False  | 8             |  46.964 ms | 0.9334 ms |  1.9067 ms |  46.719 ms |  0.34 |    0.02 |         - |         - |         - |    2136 B |       0.001 |
 UnboundedChannel      | .NET 7.0  | False  | 8             |  89.459 ms | 1.7749 ms |  3.7438 ms |  89.734 ms |  0.65 |    0.05 |         - |         - |         - |  267800 B |       0.064 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 7.0  | True   | 8             | 136.437 ms | 2.6468 ms |  5.8651 ms | 135.279 ms |  1.00 |    0.06 |         - |         - |         - |  527704 B |       1.000 |
 ConcurrentNativeQueue | .NET 7.0  | True   | 8             |  53.385 ms | 1.0430 ms |  2.0094 ms |  53.347 ms |  0.39 |    0.02 |         - |         - |         - |    2136 B |       0.004 |
 UnboundedChannel      | .NET 7.0  | True   | 8             |  86.412 ms | 1.4912 ms |  2.2320 ms |  86.330 ms |  0.63 |    0.03 |         - |         - |         - |  136280 B |       0.258 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | False  | 8             | 134.490 ms | 2.6304 ms |  4.8756 ms | 134.174 ms |  1.00 |    0.05 |         - |         - |         - | 4198488 B |       1.000 |
 ConcurrentNativeQueue | .NET 8.0  | False  | 8             |  46.096 ms | 1.1186 ms |  3.2274 ms |  45.804 ms |  0.34 |    0.03 |         - |         - |         - |    2136 B |       0.001 |
 UnboundedChannel      | .NET 8.0  | False  | 8             |  87.321 ms | 1.7198 ms |  3.9169 ms |  87.532 ms |  0.65 |    0.04 |         - |         - |         - |   37080 B |       0.009 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 8.0  | True   | 8             | 136.016 ms | 2.6408 ms |  4.5552 ms | 136.195 ms |  1.00 |    0.05 |         - |         - |         - |   68184 B |        1.00 |
 ConcurrentNativeQueue | .NET 8.0  | True   | 8             |  45.366 ms | 0.9043 ms |  1.9658 ms |  45.073 ms |  0.33 |    0.02 |         - |         - |         - |    2136 B |        0.03 |
 UnboundedChannel      | .NET 8.0  | True   | 8             |  88.316 ms | 1.7621 ms |  3.7168 ms |  87.835 ms |  0.65 |    0.03 |         - |         - |         - |   20248 B |        0.30 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | False  | 8             | 136.428 ms | 2.7208 ms |  5.3706 ms | 135.655 ms |  1.00 |    0.05 |         - |         - |         - |  265304 B |       1.000 |
 ConcurrentNativeQueue | .NET 9.0  | False  | 8             |  45.879 ms | 1.0181 ms |  2.8881 ms |  45.914 ms |  0.34 |    0.02 |         - |         - |         - |    2136 B |       0.008 |
 UnboundedChannel      | .NET 9.0  | False  | 8             |  85.688 ms | 1.6295 ms |  2.4390 ms |  85.803 ms |  0.63 |    0.03 |         - |         - |         - |   70296 B |       0.265 |
                       |           |        |               |            |           |            |            |       |         |           |           |           |           |             |
 ConcurrentQueue       | .NET 9.0  | True   | 8             | 135.649 ms | 2.5960 ms |  6.0167 ms | 134.344 ms |  1.00 |    0.06 |         - |         - |         - |  133976 B |        1.00 |
 ConcurrentNativeQueue | .NET 9.0  | True   | 8             |  44.686 ms | 0.9816 ms |  2.8322 ms |  43.795 ms |  0.33 |    0.03 |         - |         - |         - |    2136 B |        0.02 |
 UnboundedChannel      | .NET 9.0  | True   | 8             |  85.500 ms | 1.7056 ms |  3.2860 ms |  85.588 ms |  0.63 |    0.04 |         - |         - |         - |   70296 B |        0.52 |
