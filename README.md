<!-- markdownlint-disable no-inline-html no-alt-text -->

# <img src="Image/rain.ico" width="45" aligh="left"> 雨天跟打器 (.NET 8)

<p align="left">
  <a href="https://github.com/Wu-HZ/ytgdqN/releases/latest">
    <img src="https://img.shields.io/github/v/release/Wu-HZ/ytgdqN.svg?color=orange" alt="Release" />
  </a>
  <a href="./LICENSE.txt">
    <img src="https://img.shields.io/github/license/Wu-HZ/ytgdqN" alt="Apache License 2.0" />
  </a>
</p>

> **Note**: This is a .NET 8 fork of [LightAPIs/ytgdq](https://github.com/LightAPIs/ytgdq)

## 简介

雨天跟打器是一款免费开源的便携打字练习软件。本分支将原项目从 .NET Framework 4.0 迁移到 .NET 8，以获得更好的性能和现代化支持。

## 运行要求

- .NET 8.0 Desktop Runtime ([下载](https://dotnet.microsoft.com/download/dotnet/8.0))

## 使用说明

[Wiki](https://github.com/LightAPIs/ytgdq/wiki)

## .NET 8 迁移说明

本分支对原项目进行了以下现代化改造：

| 改动项 | 说明 |
|--------|------|
| 目标框架 | .NET Framework 4.0 → .NET 8.0 |
| 项目格式 | 传统 .csproj → SDK 风格 .csproj |
| 构建工具 | MSBuild (Visual Studio) → dotnet CLI |
| 快捷方式创建 | COM 组件 (IWshRuntimeLibrary) → P/Invoke (IShellLink) |
| TLS 支持 | 手动配置 → 原生支持 |
| DPI 支持 | 无 → PerMonitorV2 高清显示 |

### 构建方法

```bash
# 构建
dotnet build

# 发布 x64 版本
dotnet publish -c Release -r win-x64 --self-contained false

# 发布 x86 版本
dotnet publish -c Release -r win-x86 --self-contained false
```

## 下载地址

- [Releases](https://github.com/Wu-HZ/ytgdqN/releases)

## 致谢

- [LightAPIs/ytgdq](https://github.com/LightAPIs/ytgdq) - 原项目 ([Apache-2.0 License](https://github.com/LightAPIs/ytgdq/blob/main/LICENSE.txt))
- [taliove/tygdq](https://github.com/taliove/tygdq) - 添雨跟打器 ([Apache-2.0 License](https://github.com/taliove/tygdq/blob/master/LICENSE))

## 相关项目

- [LightAPIs/article-storage](https://github.com/LightAPIs/article-storage)

## 许可证

[Apache License 2.0](/LICENSE.txt)
