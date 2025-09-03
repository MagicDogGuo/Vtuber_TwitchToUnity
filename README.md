# TwitchToUnity - VRM虚拟主播系统

一个基于Unity和VRM的实时虚拟主播系统，支持通过Twitch聊天和面部追踪来控制VRM角色。

## 🎯 项目简介

TwitchToUnity是一个创新的虚拟主播解决方案，结合了以下技术：
- **Unity 2019.4.12f1** - 游戏引擎
- **VRM** - 虚拟角色模型格式
- **PyTorch** - 面部追踪AI模型（基于[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)项目）
- **Twitch IRC** - 实时聊天集成

系统能够实时接收面部追踪数据，并将其映射到VRM角色的BlendShape动画，同时支持通过Twitch聊天进行互动控制。

> **注意**：本项目基于[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)项目的面部追踪技术，将Live2D支持替换为VRM支持。

## 🛠️ 技术栈

### 核心组件
- **Unity 2019.4.12f1** - 主开发环境
- **VRM 0.x** - 虚拟角色模型支持
- **UniGLTF** - GLTF/GLB文件处理
- **MToon** - VRM专用着色器

### 面部追踪
- **PyTorch** - 深度学习框架（用于面部追踪模型）
- **Socket通信** - 实时数据传输
- **BlendShape驱动** - 面部表情动画

### 直播集成
- **Twitch IRC** - 聊天机器人
- **OAuth认证** - Twitch API访问
- **实时互动** - 观众参与功能

## 📋 系统要求

### 开发环境
- Unity 2019.4.12f1 或更高版本
- Windows 10/11
- Visual Studio 2019/2022 或 VS Code

### 运行时依赖
- **Python 3.8** (推荐使用Anaconda)
- **VTuber-MomoseHiyori项目** - 面部追踪核心
- 网络连接（用于Twitch集成）

### 可选依赖
- **CUDA v10.2 & CUDNN v8.3.1** (GPU加速)
- **dlib v19.24.2** (CPU环境)

## 🚀 安装说明

### 1. 克隆项目
```bash
git clone https://github.com/MagicDogGuo/Vtuber_TwitchToUnity.git
cd Vtuber_TwitchToUnity
```

### 2. 打开Unity项目
- 启动Unity Hub
- 添加项目到Unity Hub
- 使用Unity 2019.4.12f1打开项目

### 3. 安装依赖包
项目会自动安装以下Unity包：
- VRM
- UniGLTF
- TextMesh Pro
- Post Processing

### 4. 配置PyTorch环境

#### 方法一：使用VTuber-MomoseHiyori项目（推荐）

1. **克隆面部追踪项目**
```bash
git clone https://github.com/MagicDogGuo/VTuber-MomoseHiyori.git
cd VTuber-MomoseHiyori
```

2. **创建Python环境**
```bash
# 使用conda创建环境
conda env create -f environment.yml
conda activate l2d-vtb

# 安装依赖
pip install -r requirements.txt
```

3. **选择运行环境**

**CPU环境（推荐用于VTuber，FPS更高）**
```bash
# 安装dlib
conda install -c conda-forge dlib

# 测试摄像头
python main.py --debug

# 连接Unity
python main.py --debug --connect
```

**GPU环境（识别更稳定，但FPS较低）**
```bash
# Windows用户
# 安装CUDA v10.2 & CUDNN v8.3.1
pip install torch==1.10.2+cu102 torchvision==0.11.3+cu102 torchaudio===0.10.2+cu102 -f https://download.pytorch.org/whl/cu102/torch_stable.html

# MacOS (Intel)
conda install pytorch torchvision torchaudio -c pytorch

# MacOS (Apple M1)
conda install pytorch torchvision torchaudio -c pytorch-nightly

# 测试摄像头
python main.py --debug --gpu

# 连接Unity
python main.py --debug --gpu --connect
```

#### 方法二：手动安装PyTorch
```bash
# 安装Python依赖
pip install torch torchvision
pip install opencv-python
pip install numpy
```

### 5. 配置Twitch设置
1. **获取OAuth令牌**：访问 https://twitchapps.com/tmi/ 获取您的OAuth令牌
2. **配置Unity项目**：
   - 打开Unity项目中的Twitch设置UI
   - 在Password字段中输入您的OAuth令牌（格式：`oauth:your_token_here`）
   - 输入您的Twitch用户名和频道名
   - 点击Connect按钮保存设置

> **安全提醒**：OAuth令牌是敏感信息，请勿在代码中硬编码或上传到版本控制系统。项目会自动将设置保存到本地PlayerPrefs中。

## 🎮 使用方法

### 启动虚拟主播

1. **启动Python面部追踪服务**
   - 打开命令行，进入VTuber-MomoseHiyori目录
   - 激活conda环境：`conda activate l2d-vtb`
   - 运行面部追踪：
     - CPU环境：`python main.py --debug --connect`
     - GPU环境：`python main.py --debug --gpu --connect`

2. **启动Unity项目**
   - 打开Unity项目
   - 加载主场景：`Assets/_Scenes/Play_TopytorchandToTwitch.unity`
   - 配置端口号（默认：14514）
   - 运行Unity项目

3. **验证连接**
   - 确保Python服务显示"Connected to Unity"
   - Unity中应该能看到面部参数实时更新

### 场景说明

项目包含以下主要场景：

- **ToPytouch.unity** - PyTorch面部追踪集成场景
  - 包含面部追踪数据接收和VRM角色驱动
  - 用于测试面部追踪功能

- **ToTwitch.unity** - Twitch聊天集成场景  
  - 包含Twitch聊天连接和互动功能
  - 用于测试Twitch集成功能

- **Play_TopytorchandToTwitch.unity** - 完整功能场景
  - 同时集成面部追踪和Twitch聊天功能
  - 主场景，包含完整的VRM虚拟主播系统
  - 支持实时面部表情映射和观众互动

### 核心脚本

#### 面部追踪相关
- **Face/Momose.cs** - 面部数据接收和Socket通信
- **Face/FaceBlendShapeDriver.cs** - VRM面部动画驱动

#### Twitch集成相关
- **TwitchChat.cs** - Twitch聊天核心功能
- **TwitchChatSettingsUI.cs** - Twitch设置UI界面
- **ITwitchCommandHandler.cs** - Twitch命令处理接口
- **MessageEffect.cs** - 聊天消息特效

#### 其他功能
- **RPGPlayer.cs** - 角色控制
- **Particle.cs** - 粒子效果

## 🎭 VRM角色配置

### BlendShape映射
系统支持以下面部表情映射：
- 眼睛开合（左/右）
- 眉毛位置（左/右）
- 嘴巴开合
- 嘴巴宽度
- 头部旋转（X/Y/Z轴）

### 自定义BlendShape
在 `Face/FaceBlendShapeDriver.cs` 中修改BlendShape名称以匹配您的VRM模型：
```csharp
public string eyeCloseLeftName = "Face.M_F00_000_00_Fcl_EYE_Close_L";
public string eyeCloseRightName = "Face.M_F00_000_00_Fcl_EYE_Close_R";
// ... 其他BlendShape配置
```

## 🔧 高级配置

### 网络设置
- 默认端口：14514
- 本地IP：127.0.0.1
- 支持自定义端口配置

### 平滑设置
- 面部表情平滑：0-1范围
- 头部旋转平滑
- 位置偏移调整

### 调试功能
- 面部参数实时显示
- 网络连接状态监控
- BlendShape权重调试

## 📡 Twitch集成

### 聊天命令
系统支持通过Twitch聊天进行互动：
- 表情控制
- 动作触发
- 观众互动

### OAuth配置
1. 访问 https://twitchapps.com/tmi/ 获取OAuth令牌
2. 在Unity项目的Twitch设置UI中配置：
   - Password字段：输入OAuth令牌（格式：`oauth:your_token_here`）
   - Username字段：输入您的Twitch用户名
   - Channel字段：输入您的频道名
3. 点击Connect按钮保存设置

> **安全提醒**：OAuth令牌是敏感信息，请勿在代码中硬编码或上传到版本控制系统。

## 🐛 故障排除

### 常见问题

1. **端口被占用**：修改端口号或关闭占用进程
2. **VRM模型不显示**：检查BlendShape名称匹配
3. **面部追踪不工作**：
   - 确认Python服务正在运行
   - 检查摄像头是否被其他程序占用
   - 验证conda环境是否正确激活
4. **Twitch连接失败**：检查OAuth令牌和网络连接
5. **Python环境问题**：
   - 确保使用正确的Python版本（3.8）
   - 检查conda环境是否正确创建
   - 验证所有依赖包已安装
6. **构建后连接失败**：
   - 确保Unity和Python使用相同的端口号（默认14514）
   - 检查防火墙设置，确保端口未被阻止
   - 在Unity控制台查看连接状态日志
   - 确保Python服务在Unity启动前运行

### 性能优化建议

- **使用聚光灯**：让面部更亮，效果比自然光更好
- **调整面部位置**：将面部放在画面中央，不要靠近边界
- **避免戴眼镜**：可能影响眼睛识别精度
- **露出额头**：长发遮住眼睛可能影响识别

### 调试模式
启用调试日志：
```csharp
private bool enableFaceParamsDebug = true;
```

### 测试摄像头
在连接Unity之前，先测试摄像头是否正常工作：
```bash
# CPU环境
python main.py --debug

# GPU环境
python main.py --debug --gpu
```

## 📄 许可证

本项目基于开源许可证发布，具体许可证信息请查看LICENSE文件。

## 🙏 致谢

本项目基于以下开源项目：
- **[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)** - 面部追踪核心算法
- **VRM** - 虚拟角色模型格式
- **Unity** - 游戏引擎

感谢原项目作者[Kennard Wang](https://github.com/KennardWang)提供的优秀面部追踪技术。

## 🤝 贡献

欢迎提交Issue和Pull Request来改进项目！

## 📞 支持

如有问题或建议，请通过以下方式联系：
- 提交GitHub Issue
- 发送邮件至项目维护者

---

**注意**：本项目需要配置正确的OAuth令牌和网络设置才能正常工作。请确保遵循Twitch的服务条款和API使用规范。
