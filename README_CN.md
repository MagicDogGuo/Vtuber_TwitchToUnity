# TwitchToUnity - VRM虛擬主播系統

一個基於Unity和VRM的即時虛擬主播系統，支援透過Twitch聊天和面部追蹤來控制VRM角色。

> **English Version**: [README.md](README.md)

## 🎯 專案簡介

TwitchToUnity是一個創新的虛擬主播解決方案，結合了以下技術：
- **Unity 2019.4.12f1** - 遊戲引擎
- **VRM** - 虛擬角色模型格式
- **PyTorch** - 面部追蹤AI模型（基於[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)專案）
- **Twitch IRC** - 即時聊天整合

系統能夠即時接收面部追蹤資料，並將其映射到VRM角色的BlendShape動畫，同時支援透過Twitch聊天進行互動控制。

> **注意**：本專案基於[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)專案的面部追蹤技術，將Live2D支援替換為VRM支援。

## 🛠️ 技術棧

### 核心組件
- **Unity 2019.4.12f1** - 主開發環境
- **VRM 0.x** - 虛擬角色模型支援
- **UniGLTF** - GLTF/GLB檔案處理
- **MToon** - VRM專用著色器

### 面部追蹤
- **PyTorch** - 深度學習框架（用於面部追蹤模型）
- **Socket通訊** - 即時資料傳輸
- **BlendShape驅動** - 面部表情動畫

### 直播整合
- **Twitch IRC** - 聊天機器人
- **OAuth認證** - Twitch API存取
- **即時互動** - 觀眾參與功能

## 📋 系統需求

### 開發環境
- Unity 2019.4.12f1 或更高版本
- Windows 10/11
- Visual Studio 2019/2022 或 VS Code

### 執行時依賴
- **Python 3.8** (推薦使用Anaconda)
- **VTuber-MomoseHiyori專案** - 面部追蹤核心
- 網路連線（用於Twitch整合）

### 可選依賴
- **CUDA v10.2 & CUDNN v8.3.1** (GPU加速)
- **dlib v19.24.2** (CPU環境)

## 🚀 安裝說明

### 1. 複製專案
```bash
git clone https://github.com/MagicDogGuo/Vtuber_TwitchToUnity.git
cd Vtuber_TwitchToUnity
```

### 2. 開啟Unity專案
- 啟動Unity Hub
- 新增專案到Unity Hub
- 使用Unity 2019.4.12f1開啟專案

### 3. 安裝依賴套件
專案會自動安裝以下Unity套件：
- VRM
- UniGLTF
- TextMesh Pro
- Post Processing

### 4. 設定PyTorch環境

#### 方法一：使用VTuber-MomoseHiyori專案（推薦）

1. **複製面部追蹤專案**
```bash
git clone https://github.com/MagicDogGuo/VTuber-MomoseHiyori.git
cd VTuber-MomoseHiyori
```

2. **建立Python環境**
```bash
# 使用conda建立環境
conda env create -f environment.yml
conda activate l2d-vtb

# 安裝依賴
pip install -r requirements.txt
```

3. **選擇執行環境**

**CPU環境（推薦用於VTuber，FPS更高）**
```bash
# 安裝dlib
conda install -c conda-forge dlib

# 測試攝影機
python main.py --debug

# 連線Unity
python main.py --debug --connect
```

**GPU環境（識別更穩定，但FPS較低）**
```bash
# Windows使用者
# 安裝CUDA v10.2 & CUDNN v8.3.1
pip install torch==1.10.2+cu102 torchvision==0.11.3+cu102 torchaudio===0.10.2+cu102 -f https://download.pytorch.org/whl/cu102/torch_stable.html

# MacOS (Intel)
conda install pytorch torchvision torchaudio -c pytorch

# MacOS (Apple M1)
conda install pytorch torchvision torchaudio -c pytorch-nightly

# 測試攝影機
python main.py --debug --gpu

# 連線Unity
python main.py --debug --gpu --connect
```

#### 方法二：手動安裝PyTorch
```bash
# 安裝Python依賴
pip install torch torchvision
pip install opencv-python
pip install numpy
```

### 5. 設定Twitch設定
1. **取得OAuth令牌**：造訪 https://twitchapps.com/tmi/ 取得您的OAuth令牌
2. **設定Unity專案**：
   - 開啟Unity專案中的Twitch設定UI
   - 在Password欄位中輸入您的OAuth令牌（格式：`oauth:your_token_here`）
   - 輸入您的Twitch使用者名稱和頻道名稱
   - 點擊Connect按鈕儲存設定

> **安全提醒**：OAuth令牌是敏感資訊，請勿在程式碼中硬編碼或上傳到版本控制系統。專案會自動將設定儲存到本地PlayerPrefs中。

## 🎮 使用方法

### 啟動虛擬主播

1. **啟動Python面部追蹤服務**
   - 開啟命令列，進入VTuber-MomoseHiyori目錄
   - 啟動conda環境：`conda activate l2d-vtb`
   - 執行面部追蹤：
     - CPU環境：`python main.py --debug --connect`
     - GPU環境：`python main.py --debug --gpu --connect`

2. **啟動Unity專案**
   - 開啟Unity專案
   - 載入主場景：`Assets/_Scenes/Play_TopytorchandToTwitch.unity`
   - 設定連接埠號（預設：14514）
   - 執行Unity專案

3. **驗證連線**
   - 確保Python服務顯示"Connected to Unity"
   - Unity中應該能看到面部參數即時更新

### 場景說明

專案包含以下主要場景：

- **ToPytouch.unity** - PyTorch面部追蹤整合場景
  - 包含面部追蹤資料接收和VRM角色驅動
  - 用於測試面部追蹤功能

- **ToTwitch.unity** - Twitch聊天整合場景  
  - 包含Twitch聊天連線和互動功能
  - 用於測試Twitch整合功能

- **Play_TopytorchandToTwitch.unity** - 完整功能場景
  - 同時整合面部追蹤和Twitch聊天功能
  - 主場景，包含完整的VRM虛擬主播系統
  - 支援即時面部表情映射和觀眾互動

### 核心腳本

#### 面部追蹤相關
- **Face/Momose.cs** - 面部資料接收和Socket通訊
- **Face/FaceBlendShapeDriver.cs** - VRM面部動畫驅動

#### Twitch整合相關
- **TwitchChat.cs** - Twitch聊天核心功能
- **TwitchChatSettingsUI.cs** - Twitch設定UI介面
- **ITwitchCommandHandler.cs** - Twitch命令處理介面
- **MessageEffect.cs** - 聊天訊息特效

#### 其他功能
- **RPGPlayer.cs** - 角色控制
- **Particle.cs** - 粒子效果

## 🎭 VRM角色設定

### BlendShape映射
系統支援以下面部表情映射：
- 眼睛開合（左/右）
- 眉毛位置（左/右）
- 嘴巴開合
- 嘴巴寬度
- 頭部旋轉（X/Y/Z軸）

### 自訂BlendShape
在 `Face/FaceBlendShapeDriver.cs` 中修改BlendShape名稱以匹配您的VRM模型：
```csharp
public string eyeCloseLeftName = "Face.M_F00_000_00_Fcl_EYE_Close_L";
public string eyeCloseRightName = "Face.M_F00_000_00_Fcl_EYE_Close_R";
// ... 其他BlendShape設定
```

## 🔧 進階設定

### 網路設定
- 預設連接埠：14514
- 本地IP：127.0.0.1
- 支援自訂連接埠設定

### 平滑設定
- 面部表情平滑：0-1範圍
- 頭部旋轉平滑
- 位置偏移調整

### 除錯功能
- 面部參數即時顯示
- 網路連線狀態監控
- BlendShape權重除錯

## 📡 Twitch整合

### 聊天命令
系統支援透過Twitch聊天進行互動：
- 表情控制
- 動作觸發
- 觀眾互動

### OAuth設定
1. 造訪 https://twitchapps.com/tmi/ 取得OAuth令牌
2. 在Unity專案的Twitch設定UI中設定：
   - Password欄位：輸入OAuth令牌（格式：`oauth:your_token_here`）
   - Username欄位：輸入您的Twitch使用者名稱
   - Channel欄位：輸入您的頻道名稱
3. 點擊Connect按鈕儲存設定

> **安全提醒**：OAuth令牌是敏感資訊，請勿在程式碼中硬編碼或上傳到版本控制系統。

## 🐛 故障排除

### 常見問題

1. **連接埠被佔用**：修改連接埠號或關閉佔用程序
2. **VRM模型不顯示**：檢查BlendShape名稱匹配
3. **面部追蹤不工作**：
   - 確認Python服務正在執行
   - 檢查攝影機是否被其他程式佔用
   - 驗證conda環境是否正確啟動
4. **Twitch連線失敗**：檢查OAuth令牌和網路連線
5. **Python環境問題**：
   - 確保使用正確的Python版本（3.8）
   - 檢查conda環境是否正確建立
   - 驗證所有依賴套件已安裝
6. **建置後連線失敗**：
   - 確保Unity和Python使用相同的連接埠號（預設14514）
   - 檢查防火牆設定，確保連接埠未被阻擋
   - 在Unity控制台查看連線狀態日誌
   - 確保Python服務在Unity啟動前執行

### 效能最佳化建議

- **使用聚光燈**：讓面部更亮，效果比自然光更好
- **調整面部位置**：將面部放在畫面中央，不要靠近邊界
- **避免戴眼鏡**：可能影響眼睛識別精度
- **露出額頭**：長髮遮住眼睛可能影響識別

### 除錯模式
啟用除錯日誌：
```csharp
private bool enableFaceParamsDebug = true;
```

### 測試攝影機
在連線Unity之前，先測試攝影機是否正常工作：
```bash
# CPU環境
python main.py --debug

# GPU環境
python main.py --debug --gpu
```

## 📄 授權

本專案基於開源授權發布，具體授權資訊請查看LICENSE檔案。

## 🙏 致謝

本專案基於以下開源專案：
- **[VTuber-MomoseHiyori](https://github.com/MagicDogGuo/VTuber-MomoseHiyori)** - 面部追蹤核心演算法
- **VRM** - 虛擬角色模型格式
- **Unity** - 遊戲引擎

感謝原專案作者[Kennard Wang](https://github.com/KennardWang)提供的優秀面部追蹤技術。

## 🤝 貢獻

歡迎提交Issue和Pull Request來改進專案！

## 📞 支援

如有問題或建議，請透過以下方式聯絡：
- 提交GitHub Issue
- 發送郵件至專案維護者

---

**注意**：本專案需要設定正確的OAuth令牌和網路設定才能正常工作。請確保遵循Twitch的服務條款和API使用規範。
