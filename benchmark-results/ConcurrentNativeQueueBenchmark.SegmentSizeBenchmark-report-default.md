
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

 Method        | Runtime   | Server | Count | Mean       | Error      | StdDev     | Median     | Ratio | RatioSD | Allocated | Alloc Ratio |
-------------- |---------- |------- |------ |-----------:|-----------:|-----------:|-----------:|------:|--------:|----------:|------------:|
 **SmallSegments** | **.NET 10.0** | **False**  | **1024**  |  **13.594 μs** |  **0.5868 μs** |  **1.6357 μs** |  **13.300 μs** |  **1.01** |    **0.17** |         **-** |          **NA** |
 LargeSegments | .NET 10.0 | False  | 1024  |  11.845 μs |  0.4599 μs |  1.2896 μs |  11.400 μs |  0.88 |    0.14 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 10.0 | True   | 1024  |  12.604 μs |  0.5285 μs |  1.4734 μs |  11.900 μs |  1.01 |    0.16 |         - |          NA |
 LargeSegments | .NET 10.0 | True   | 1024  |  12.130 μs |  0.6657 μs |  1.9206 μs |  11.550 μs |  0.97 |    0.18 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 6.0  | False  | 1024  |   8.899 μs |  0.1961 μs |  0.5628 μs |   8.900 μs |  1.00 |    0.09 |         - |          NA |
 LargeSegments | .NET 6.0  | False  | 1024  |   7.891 μs |  0.1808 μs |  0.5303 μs |   7.700 μs |  0.89 |    0.08 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 6.0  | True   | 1024  |   9.854 μs |  0.2121 μs |  0.6255 μs |   9.900 μs |  1.00 |    0.09 |         - |          NA |
 LargeSegments | .NET 6.0  | True   | 1024  |   8.545 μs |  0.1733 μs |  0.4655 μs |   8.600 μs |  0.87 |    0.07 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 7.0  | False  | 1024  |  11.892 μs |  0.4331 μs |  1.2565 μs |  11.500 μs |  1.01 |    0.15 |         - |          NA |
 LargeSegments | .NET 7.0  | False  | 1024  |  10.385 μs |  0.2077 μs |  0.4975 μs |  10.150 μs |  0.88 |    0.10 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 7.0  | True   | 1024  |  12.778 μs |  0.3572 μs |  1.0476 μs |  12.600 μs |  1.01 |    0.12 |         - |          NA |
 LargeSegments | .NET 7.0  | True   | 1024  |  12.365 μs |  0.4005 μs |  1.1683 μs |  12.100 μs |  0.97 |    0.12 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 8.0  | False  | 1024  |  13.487 μs |  0.6635 μs |  1.8824 μs |  13.200 μs |  1.02 |    0.19 |         - |          NA |
 LargeSegments | .NET 8.0  | False  | 1024  |  12.013 μs |  0.5733 μs |  1.6357 μs |  11.550 μs |  0.91 |    0.17 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 8.0  | True   | 1024  |  14.586 μs |  0.5469 μs |  1.5336 μs |  14.300 μs |  1.01 |    0.15 |         - |          NA |
 LargeSegments | .NET 8.0  | True   | 1024  |  13.031 μs |  0.4211 μs |  1.2083 μs |  12.900 μs |  0.90 |    0.12 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 9.0  | False  | 1024  |  13.406 μs |  0.5352 μs |  1.4920 μs |  13.100 μs |  1.01 |    0.15 |         - |          NA |
 LargeSegments | .NET 9.0  | False  | 1024  |  12.739 μs |  0.4322 μs |  1.2192 μs |  12.650 μs |  0.96 |    0.13 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 9.0  | True   | 1024  |  12.734 μs |  0.5202 μs |  1.4671 μs |  12.000 μs |  1.01 |    0.16 |         - |          NA |
 LargeSegments | .NET 9.0  | True   | 1024  |  11.513 μs |  0.5428 μs |  1.5040 μs |  11.000 μs |  0.92 |    0.15 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 **SmallSegments** | **.NET 10.0** | **False**  | **65536** | **626.806 μs** | **10.1972 μs** | **10.4718 μs** | **624.700 μs** |  **1.00** |    **0.02** |         **-** |          **NA** |
 LargeSegments | .NET 10.0 | False  | 65536 | 642.107 μs |  6.2415 μs |  5.5329 μs | 642.150 μs |  1.02 |    0.02 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 10.0 | True   | 65536 | 616.046 μs |  8.2441 μs |  6.8842 μs | 615.700 μs |  1.00 |    0.02 |         - |          NA |
 LargeSegments | .NET 10.0 | True   | 65536 | 626.687 μs |  9.2579 μs |  8.6598 μs | 624.300 μs |  1.02 |    0.02 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 6.0  | False  | 65536 | 612.529 μs |  4.2680 μs |  3.7835 μs | 612.100 μs |  1.00 |    0.01 |         - |          NA |
 LargeSegments | .NET 6.0  | False  | 65536 | 613.447 μs | 11.4004 μs | 10.6639 μs | 610.800 μs |  1.00 |    0.02 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 6.0  | True   | 65536 | 619.013 μs |  7.3985 μs |  6.9206 μs | 616.100 μs |  1.00 |    0.02 |         - |          NA |
 LargeSegments | .NET 6.0  | True   | 65536 | 602.431 μs |  2.6837 μs |  2.2411 μs | 602.800 μs |  0.97 |    0.01 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 7.0  | False  | 65536 | 718.543 μs | 12.7876 μs | 18.3396 μs | 714.400 μs |  1.00 |    0.04 |         - |          NA |
 LargeSegments | .NET 7.0  | False  | 65536 | 712.331 μs |  9.5770 μs |  7.9972 μs | 713.400 μs |  0.99 |    0.03 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 7.0  | True   | 65536 | 699.550 μs |  3.1659 μs |  2.8065 μs | 700.050 μs |  1.00 |    0.01 |         - |          NA |
 LargeSegments | .NET 7.0  | True   | 65536 | 700.350 μs | 12.7625 μs | 11.3136 μs | 695.500 μs |  1.00 |    0.02 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 8.0  | False  | 65536 | 640.853 μs | 11.5101 μs | 19.8543 μs | 635.600 μs |  1.00 |    0.04 |         - |          NA |
 LargeSegments | .NET 8.0  | False  | 65536 | 628.408 μs |  7.6612 μs |  6.3975 μs | 628.500 μs |  0.98 |    0.03 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 8.0  | True   | 65536 | 618.815 μs |  8.4939 μs |  7.0928 μs | 615.700 μs |  1.00 |    0.02 |         - |          NA |
 LargeSegments | .NET 8.0  | True   | 65536 | 619.185 μs |  4.8491 μs |  4.0492 μs | 619.300 μs |  1.00 |    0.01 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 9.0  | False  | 65536 | 640.971 μs | 12.7428 μs | 20.9368 μs | 632.000 μs |  1.00 |    0.05 |         - |          NA |
 LargeSegments | .NET 9.0  | False  | 65536 | 626.547 μs | 10.6439 μs | 11.8307 μs | 624.300 μs |  0.98 |    0.04 |         - |          NA |
               |           |        |       |            |            |            |            |       |         |           |             |
 SmallSegments | .NET 9.0  | True   | 65536 | 627.143 μs |  6.8216 μs |  6.0472 μs | 628.000 μs |  1.00 |    0.01 |         - |          NA |
 LargeSegments | .NET 9.0  | True   | 65536 | 618.793 μs |  7.7334 μs |  7.2338 μs | 619.100 μs |  0.99 |    0.01 |         - |          NA |
