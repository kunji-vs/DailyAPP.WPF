

# DailyAPP.WPF

一个基于 WPF 和 Prism 框架开发的日常事务管理桌面应用程序。

## 项目简介

DailyAPP.WPF 是一款简洁实用的日常事务管理工具，支持待办事项管理、备忘录记录、个人设置等功能。采用 MVVM 架构设计，使用 Prism 框架实现模块化和依赖注入，界面采用 MaterialDesign 美观大方。

## 技术栈

- **框架**: WPF (.NET)
- **MVVM 框架**: Prism.Unity
- **UI 组件库**: MaterialDesign
- **HTTP 客户端**: RestSharp
- **对象映射**: AutoMapper
- **事件总线**: Prism EventAggregator

## 功能特性

### 用户模块
- 用户登录/注册
- 账户信息管理

### 主要功能
- **首页 Dashboard**: 展示待办事项统计、备忘录概览
- **待办事项**: 创建、查看、编辑待办任务
- **备忘录**: 记录和管理日常备忘录
- **个人中心**: 主题切换、用户信息管理
- **系统设置**: 应用配置

### 界面特性
- 窗口控制（最大化、最小化、关闭）
- 主题颜色切换
- 侧边栏导航
- 抽屉式面板

## 项目结构

```
DailyAPP.WPF/
├── AutoMappers/          # AutoMapper 配置
├── Converters/          # 值转换器
├── DTOs/                # 数据传输对象
│   ├── AccountInfoDTO   # 账户信息
│   ├── MemoInfoDTO      # 备忘录信息
│   ├── WaitInfoDTO      # 待办事项信息
│   └── ...
├── Events/              # 事件定义
├── Extensions/          # 扩展方法
├── HttpClient/          # HTTP 客户端封装
├── Images/              # 资源图片
├── Models/              # 数据模型
├── ViewModels/          # 视图模型
└── Views/               # 视图界面
```

## 快速开始

### 环境要求

- .NET 6.0 或更高版本
- Visual Studio 2022

### 编译运行

1. 克隆项目
2. 使用 Visual Studio 打开 `DailyAPP.WPF.sln`
3. 还原 NuGet 包
4. 编译并运行

### 配置说明

在 `App.xaml.cs` 中配置依赖注入和模块注册。

## 界面预览

应用包含以下主要界面：
- 登录/注册窗口
- 主窗口（含侧边导航栏）
- 首页仪表盘
- 待办事项列表
- 备忘录列表
- 个人中心
- 系统设置
- 关于我们

## 技术亮点

1. **Prism 框架**: 实现了完整的 MVVM 架构，包括导航、依赖注入、模块化
2. **MaterialDesign**: 现代化 UI 设计，支持主题定制
3. **事件总线**: 使用 PubSubEvent 实现松耦合的组件通信
4. **HTTP 封装**: 统一的 API 请求/响应处理
5. **值转换器**: 灵活的数据绑定转换

## 许可证

本项目仅供学习交流使用。
