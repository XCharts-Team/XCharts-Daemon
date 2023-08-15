# XCharts-Daemon

XCharts的守护程序，主要用来解决本地开启TextMeshPro和NewInputSystem后，更新到最新XCharts版本时不会编译失败的问题。特别是用CI-CD等自动化更新时，建议使用。

具体情况可查看 [Issue 272](https://github.com/XCharts-Team/XCharts/issues/272)

用法：

1. 将本仓库通过源码或Unity Package Manager方式加入项目。
2. 更新XCharts版本时不要删掉改守护程序，更新完成后守护程序会自动处理XCharts的asmdef，确保引用正常。
