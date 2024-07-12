# Multi-User Mixed-Reality Coordination System

## Introduction

This project will establish a end-to-end mixed reality interactive system for multiple people to interact on-site, with the possibility of remote third parties joining for coordination. Using the coordination between construction site engineers as an example, this project can instantly update the Building Information Model (BIM) with modifications made to 3D models during coordindations, thereby improving collaboration efficiency.

# Demo

The image below links to our demo video. As you can see in the video, the movement of another MR user is observable, and within the same space, there are objects that have been aligned and adjusted.

<a href="https://www.youtube.com/watch?v=M2ibKEc0ke4&ab_channel=huai-encheng" title="Watch the video">
    <img src="https://github.com/ryanchenggg/Multi-User-Mixed-Reality-Coordination-System/blob/main/img/coordination.png" alt="Watch the video" width="600" height="400">
</a>

## Prerequisites
### Recommended Tool Versions

To ensure the best performance and compatibility, it is recommended to use the following software versions for development on HoloLens 2:

- **Visual Studio**: Latest version of Visual Studio 2022 or 2019. Make sure to install the **Universal Windows Platform development** workload and the **Windows 10 SDK (10.0.18362.0 or newer)**.
- **Unity**: Latest Unity 2020.3 LTS. Check [Unity's blocking bugs for HoloLens 2](https://docs.unity3d.com/Manual/UnityForHoloLens.html) before starting.
- **Unity OpenXR Plugin**: Version 1.3.1 or newer recommended.
- **Mixed Reality OpenXR Plugin**: Version 1.4.0 or newer. See the [latest release notes](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/native/openxr-getting-started) for more details.
- **MRTK-Unity**: Version 2.7.3 or newer. This is crucial for optimal interaction design and development in mixed reality.
- **Windows Mixed Reality Runtime**: Version 109 or newer recommended.

### Azure Spatial Anchors (ASA)

- **ASA SDK Core**: Version 2.11.0 or newer.
- **ASA SDK for Windows**: Version 2.11.0 or newer.
- note: ASA SDK 2.9 and asset 2.4.0 have known conflicts (consider ASA SDK 2.8.1 for stability).

### Photon

- **Photon Unity Networking (PUN)**: [PUN 2 - FREE from Unity Asset Store](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922).
- **MRTK Extensions for Photon**: These are crucial for enabling multi-user capabilities and should match the MRTK version you are using.

## Contribution

1. Development of a on-site multi-user coordination system
2. Real-time feedback of coordination results to PC-based application software

## System workflow

In this project, the first step is to align the objects' positions in real-world space. To ensure consistency of object positions across all devices, we use Azure Spatial Anchors to establish anchor points. Finally, we utilize the Photon Networking connection module to synchronize user information and changes in model appearance between users.


<img src="https://github.com/ryanchenggg/Multi-User-Mixed-Reality-Coordination-System/blob/main/img/connectionsyswf.png" width="800px">

## End-to-end MR-to-PC feedback

After coordination and communication, this project implements an end-to-end feedback module that provides feedback of the results from the model of onsite or remote multi-user coordination interactions to a PC (using BIM Revit software as an example in this project, we aligned the modified model with the original to compare the differences).

<img src="https://github.com/ryanchenggg/Multi-User-Mixed-Reality-Coordination-System/blob/main/img/mr2bim.png" width="800px">



