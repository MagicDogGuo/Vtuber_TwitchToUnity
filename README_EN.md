# TwitchToUnity - VRM Virtual Streamer System

A real-time virtual streamer system based on Unity and VRM, supporting VRM character control through Twitch chat and facial tracking.

## 🎯 Project Overview

TwitchToUnity is an innovative virtual streamer solution that combines the following technologies:
- **Unity 2019.4.12f1** - Game Engine
- **VRM** - Virtual Character Model Format
- **PyTorch** - Facial Tracking AI Model (based on [VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori) project)
- **Twitch IRC** - Real-time Chat Integration

The system can receive facial tracking data in real-time and map it to VRM character BlendShape animations, while supporting interactive control through Twitch chat.

> **Note**: This project is based on the facial tracking technology from the [VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori) project, replacing Live2D support with VRM support.

## 🛠️ Technology Stack

### Core Components
- **Unity 2019.4.12f1** - Main Development Environment
- **VRM 0.x** - Virtual Character Model Support
- **UniGLTF** - GLTF/GLB File Processing
- **MToon** - VRM-specific Shader

### Facial Tracking
- **PyTorch** - Deep Learning Framework (for facial tracking models)
- **Socket Communication** - Real-time Data Transmission
- **BlendShape Driver** - Facial Expression Animation

### Live Streaming Integration
- **Twitch IRC** - Chat Bot
- **OAuth Authentication** - Twitch API Access
- **Real-time Interaction** - Audience Participation Features

## 📋 System Requirements

### Development Environment
- Unity 2019.4.12f1 or higher
- Windows 10/11
- Visual Studio 2019/2022 or VS Code

### Runtime Dependencies
- **Python 3.8** (Anaconda recommended)
- **VTuber-MomoseHiyori Project** - Facial Tracking Core
- Network connection (for Twitch integration)

### Optional Dependencies
- **CUDA v10.2 & CUDNN v8.3.1** (GPU acceleration)
- **dlib v19.24.2** (CPU environment)

## 🚀 Installation Guide

### 1. Clone the Project
```bash
git clone https://github.com/MagicDogGuo/Vtuber_TwitchToUnity.git
cd Vtuber_TwitchToUnity
```

### 2. Open Unity Project
- Launch Unity Hub
- Add project to Unity Hub
- Open project with Unity 2019.4.12f1

### 3. Install Dependencies
The project will automatically install the following Unity packages:
- VRM
- UniGLTF
- TextMesh Pro
- Post Processing

### 4. Configure PyTorch Environment

#### Method 1: Using VTuber-MomoseHiyori Project (Recommended)

1. **Clone Facial Tracking Project**
```bash
git clone https://github.com/MagicDogGuo/VTuber-MomoseHiyori.git
cd VTuber-MomoseHiyori
```

2. **Create Python Environment**
```bash
# Create environment using conda
conda env create -f environment.yml
conda activate l2d-vtb

# Install dependencies
pip install -r requirements.txt
```

3. **Choose Runtime Environment**

**CPU Environment (Recommended for VTuber, Higher FPS)**
```bash
# Install dlib
conda install -c conda-forge dlib

# Test camera
python main.py --debug

# Connect to Unity
python main.py --debug --connect
```

**GPU Environment (More Stable Recognition, Lower FPS)**
```bash
# Windows Users
# Install CUDA v10.2 & CUDNN v8.3.1
pip install torch==1.10.2+cu102 torchvision==0.11.3+cu102 torchaudio===0.10.2+cu102 -f https://download.pytorch.org/whl/cu102/torch_stable.html

# MacOS (Intel)
conda install pytorch torchvision torchaudio -c pytorch

# MacOS (Apple M1)
conda install pytorch torchvision torchaudio -c pytorch-nightly

# Test camera
python main.py --debug --gpu

# Connect to Unity
python main.py --debug --gpu --connect
```

#### Method 2: Manual PyTorch Installation
```bash
# Install Python dependencies
pip install torch torchvision
pip install opencv-python
pip install numpy
```

### 5. Configure Twitch Settings
1. **Get OAuth Token**: Visit https://twitchapps.com/tmi/ to get your OAuth token
2. **Configure Unity Project**:
   - Open Twitch settings UI in Unity project
   - Enter your OAuth token in Password field (format: `oauth:your_token_here`)
   - Enter your Twitch username and channel name
   - Click Connect button to save settings

> **Security Reminder**: OAuth tokens are sensitive information. Do not hardcode them in code or upload to version control systems. The project will automatically save settings to local PlayerPrefs.

## 🎮 Usage Guide

### Starting Virtual Streamer

1. **Start Python Facial Tracking Service**
   - Open command line, navigate to VTuber-MomoseHiyori directory
   - Activate conda environment: `conda activate l2d-vtb`
   - Run facial tracking:
     - CPU environment: `python main.py --debug --connect`
     - GPU environment: `python main.py --debug --gpu --connect`

2. **Start Unity Project**
   - Open Unity project
   - Load main scene: `Assets/_Scenes/Play_TopytorchandToTwitch.unity`
   - Configure port number (default: 14514)
   - Run Unity project

3. **Verify Connection**
   - Ensure Python service shows "Connected to Unity"
   - Unity should display real-time facial parameter updates

### Scene Description

The project includes the following main scenes:

- **ToPytouch.unity** - PyTorch Facial Tracking Integration Scene
  - Contains facial tracking data reception and VRM character driving
  - Used for testing facial tracking functionality

- **ToTwitch.unity** - Twitch Chat Integration Scene
  - Contains Twitch chat connection and interaction features
  - Used for testing Twitch integration functionality

- **Play_TopytorchandToTwitch.unity** - Complete Functionality Scene
  - Integrates both facial tracking and Twitch chat functionality
  - Main scene containing complete VRM virtual streamer system
  - Supports real-time facial expression mapping and audience interaction

### Core Scripts

#### Facial Tracking Related
- **Face/Momose.cs** - Facial Data Reception and Socket Communication
- **Face/FaceBlendShapeDriver.cs** - VRM Facial Animation Driver

#### Twitch Integration Related
- **TwitchChat.cs** - Twitch Chat Core Functionality
- **TwitchChatSettingsUI.cs** - Twitch Settings UI Interface
- **ITwitchCommandHandler.cs** - Twitch Command Handler Interface
- **MessageEffect.cs** - Chat Message Effects

#### Other Features
- **RPGPlayer.cs** - Character Control
- **Particle.cs** - Particle Effects

## 🎭 VRM Character Configuration

### BlendShape Mapping
The system supports the following facial expression mappings:
- Eye opening/closing (left/right)
- Eyebrow position (left/right)
- Mouth opening/closing
- Mouth width
- Head rotation (X/Y/Z axis)

### Custom BlendShape
Modify BlendShape names in `Face/FaceBlendShapeDriver.cs` to match your VRM model:
```csharp
public string eyeCloseLeftName = "Face.M_F00_000_00_Fcl_EYE_Close_L";
public string eyeCloseRightName = "Face.M_F00_000_00_Fcl_EYE_Close_R";
// ... other BlendShape configurations
```

## 🔧 Advanced Configuration

### Network Settings
- Default port: 14514
- Local IP: 127.0.0.1
- Supports custom port configuration

### Smoothing Settings
- Facial expression smoothing: 0-1 range
- Head rotation smoothing
- Position offset adjustment

### Debug Features
- Real-time facial parameter display
- Network connection status monitoring
- BlendShape weight debugging

## 📡 Twitch Integration

### Chat Commands
The system supports interaction through Twitch chat:
- Expression control
- Action triggering
- Audience interaction

### OAuth Configuration
1. Visit https://twitchapps.com/tmi/ to get OAuth token
2. Configure in Unity project's Twitch settings UI:
   - Password field: Enter OAuth token (format: `oauth:your_token_here`)
   - Username field: Enter your Twitch username
   - Channel field: Enter your channel name
3. Click Connect button to save settings

> **Security Reminder**: OAuth tokens are sensitive information. Do not hardcode them in code or upload to version control systems.

## 🐛 Troubleshooting

### Common Issues

1. **Port Occupied**: Modify port number or close occupying processes
2. **VRM Model Not Displaying**: Check BlendShape name matching
3. **Facial Tracking Not Working**:
   - Confirm Python service is running
   - Check if camera is occupied by other programs
   - Verify conda environment is correctly activated
4. **Twitch Connection Failed**: Check OAuth token and network connection
5. **Python Environment Issues**:
   - Ensure correct Python version (3.8)
   - Check if conda environment is correctly created
   - Verify all dependency packages are installed
6. **Build Connection Failed**:
   - Ensure Unity and Python use same port number (default 14514)
   - Check firewall settings, ensure port is not blocked
   - View connection status logs in Unity console
   - Ensure Python service runs before Unity starts

### Performance Optimization Tips

- **Use Spotlight**: Brighter facial lighting works better than natural light
- **Adjust Facial Position**: Place face in center of frame, avoid boundaries
- **Avoid Glasses**: May affect eye recognition accuracy
- **Expose Forehead**: Long hair covering eyes may affect recognition

### Debug Mode
Enable debug logging:
```csharp
private bool enableFaceParamsDebug = true;
```

### Test Camera
Test camera functionality before connecting to Unity:
```bash
# CPU environment
python main.py --debug

# GPU environment
python main.py --debug --gpu
```

## 📄 License

This project is released under open source license. Please check LICENSE file for specific license information.

## 🙏 Acknowledgments

This project is based on the following open source projects:
- **[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)** - Facial Tracking Core Algorithm
- **VRM** - Virtual Character Model Format
- **Unity** - Game Engine

Thanks to original project author [Kennard Wang](https://github.com/KennardWang) for providing excellent facial tracking technology.

## 🤝 Contributing

Welcome to submit Issues and Pull Requests to improve the project!

## 📞 Support

For questions or suggestions, please contact through:
- Submit GitHub Issue
- Send email to project maintainer

---

**Note**: This project requires correct OAuth token and network settings to function properly. Please ensure compliance with Twitch's terms of service and API usage guidelines.
