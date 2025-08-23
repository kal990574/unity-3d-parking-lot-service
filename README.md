# Unity 3D Parking-Lot Service

유니티(Unity) 기반으로 제작한 **3D 주차장 관제 서비스** 프로젝트입니다.  
실시간 API 데이터를 받아와 주차장의 **슬롯 상태**와 **차량 배치**를 시각화하며,  
UI 조작을 통한 **카메라 시점 전환**과 **자동 갱신** 기능을 제공합니다.

---

## ✨ 주요 기능 (Features)

- **API 연동 (UnityWebRequest)**  
  - 외부 서버에서 주차 상태 데이터를 가져오기  
  - 에러 처리 및 응답 코드 확인 기능 포함  

- **데이터 갱신 (Coroutine)**  
  - 일정 주기로 서버 데이터를 갱신하여 UI에 반영  

- **JSON 파싱 (Newtonsoft.Json)**  
  - `JObject`, `JToken`을 활용한 동적 키 파싱  

- **카메라 컨트롤 (Animator + UI Canvas)**  
  - 버튼 입력 → Animator Trigger → 시점 전환  

- **주차면/차량 시각화**  
  - 슬롯 프리팹을 동적으로 배치 및 색상/상태 변경  

---

## 📸 데모 (Demo)

[![Watch the video](https://img.youtube.com/vi/adHMPoeKwEE/0.jpg)](https://youtu.be/adHMPoeKwEE)

- 상단 버튼 클릭으로 카메라 전환  
- 주기적인 데이터 갱신으로 주차장 상태 자동 업데이트  

---

## ⚙️ 설치 & 실행 방법 (Getting Started)

1. **환경 요구사항**
   - Unity 2021 LTS 이상  
   - Newtonsoft.Json (Unity 패키지 매니저 설치)  

2. **설치**
   ```bash
   git clone https://github.com/kal990574/unity-3d-parking-lot-service.git
