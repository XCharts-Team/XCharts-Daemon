# XCharts-Daemon

XCharts-Daemon ensures that XCharts builds properly when version updates are made, which is useful when TextMeshPro or NewInputSystem is enabled locally. After importing XCharts-Daemon into a project, when updating XCharts, the Daemon will automatically refresh asmdef according to the status of local TMP etc., to ensure normal compilation and facilitate the execution of automated processes such as CI-CD.

The updated version of TextMeshPro after opening the compilation error is a long-standing problem, the specific situation can be seen:

* [Issue 272:  New Input System & Text Mesh Pro Library Rebuilt issue](https://github.com/XCharts-Team/XCharts/issues/272)
* [Issue 194: questions about using TextMeshPro](https://github.com/XCharts-Team/XCharts/issues/194)
* [Issue 125: Lost TMP reference on the new version?](https://github.com/XCharts-Team/XCharts/issues/125)

Usage:

1. Add the repository to the project through source code or Unity Package Manager.
2. Do not delete the daemon when updating the XCharts version. After the update is complete, the daemon will automatically handle the asmdef of XCharts to ensure that the reference is normal.

守护程序[XCharts-Daemon](https://github.com/XCharts-Team/XCharts-Daemon)可以确保XCharts版本更新时编译正常，当本地开启TextMeshPro或NewInputSystem时将会非常有用。将XCharts-Daemon导入项目后，在更新XCharts时守护程序会自动根据本地TMP等的开启情况刷新asmdef，确保编译正常，不用手动去解决，方便CI-CD等自动化流程执行。

开启TextMeshPro后的更新版本编译报错是一个存在已久的问题，具体情况可查看：

* [Issue 272: New Input System & Text Mesh Pro Library Rebuilt issue](https://github.com/XCharts-Team/XCharts/issues/272)
* [Issue 194: 关于使用TextMeshPro的问题](https://github.com/XCharts-Team/XCharts/issues/194)
* [Issue 125: Lost TMP reference on the new version?](https://github.com/XCharts-Team/XCharts/issues/125)

用法：

1. 将本仓库通过源码或Unity Package Manager方式加入项目。
2. 更新XCharts版本时不要删掉该守护程序，更新完成后守护程序会自动处理XCharts的asmdef，确保引用正常。
