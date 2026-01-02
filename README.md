# Immersive Body-Tracked Air Drawing

![Unity](https://img.shields.io/badge/Unity-2019%2B-black)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![Hardware](https://img.shields.io/badge/Hardware-Azure%20Kinect%20DK-0078D4)

> 一個結合 Azure Kinect 骨架追蹤技術的互動式 3D 空中繪畫系統。將使用者的指尖作為虛擬畫筆，在聚光燈照耀的黑色空間裡揮灑創造力。

## 專案簡介 (Introduction)

本專案基於 **Microsoft Azure Kinect Body Tracking SDK** 開發，嘗試創造一種「表演藝術」般的體驗。

場景設計模擬一個被聚光燈聚焦的黑暗舞台，使用者站在中央，透過右手食指直接在 3D 空間中書寫發光的線條。系統會即時追蹤全身骨架，並透過手勢邏輯來控制筆觸的生成。

---

## 操作說明 (Controls)

本專案採用雙手協作的邏輯，模擬畫家拿著畫板與畫筆的姿態：

| 身體部位 | 動作 | 功能說明 |
| :--- | :--- | :--- |
| **左手 (Left Hand)** | **舉起 / 放下** | **畫筆開關 (Trigger)**<br>舉起左手即開啟畫筆模式；放下則停止出墨。 |
| **右手 (Right Hand)** | **移動揮舞** | **繪製筆觸 (Brush)**<br>在左手舉起期間，右手軌跡將轉化為 3D 線條。 |
| **UI 介面** | **點擊按鈕** | **清空畫布 (Clear Canvas)**<br>移除場景中所有筆畫。 |

---

## 安裝與設定 (Installation & Setup) - 重要！若沒完成會噴錯。

**在第一次執行專案之前，請務必完成以下步驟，否則骨架追蹤將無法啟動。**

由於 Azure Kinect SDK 需要依賴原生的 DLL 檔案，Unity 在編輯器模式下執行時，需要這些檔案位於專案根目錄。

### 步驟 1：Clone 專案
將本專案下載至本地端。

### 步驟 2：複製 Plugins
打開專案資料夾，進行以下檔案搬移操作：

1.  進入 `Assets/Plugins` 資料夾。
2.  **複製** 資料夾內所有的 `.dll` 檔案（包含 Azure Kinect 相關、ONNX Runtime、DirectML 等）。
3.  回到 **專案根目錄**（即包含 `Assets`, `Library`, `Packages` 的那一層）。
4.  **貼上** 這些檔案。

### 步驟 3：硬體連接
1.  連接 Azure Kinect DK 電源與 USB 3.0 線材。
2.  確認電腦已安裝 [Azure Kinect Body Tracking SDK](https://docs.microsoft.com/en-us/azure/kinect-dk/body-sdk-setup)。
3.  開啟 Unity 場景並按下 Play。

---

## 技術需求 (Requirements)

* **作業系統**: Windows 10 / 11 (64-bit)
* **硬體設備**: Azure Kinect DK
* **顯卡需求**: 建議 NVIDIA GeForce GTX 1050 或更高（用於 CUDA 加速骨架運算）
* **開發環境**: Unity (建議 2019.4 LTS 或更高版本)

---

## 專案結構與來源

本專案延伸自官方範例 [Azure-Kinect-Samples](https://github.com/microsoft/Azure-Kinect-Samples)，並進行了以下客製化修改：

* **Scripts/**:
    * 新增筆刷控制邏輯 (`Brush.cs`, `AirControl.cs`)。
    * 新增 UI Button 以一鍵清空畫布，並掛上控制腳本（`Clear.cs`）。
* **Scenes/**:
    * 重新設計光影場景。
* **Rendering**:
    * 加入 LineRenderer 處理與發光材質設定。
* **Prefab**:
    * 將 stroke 設為 Prefab ，讓每次筆觸都生成一個新物件。

---

## License

本專案遵循原始 Azure Kinect Sample 的授權協議 (MIT License)。
