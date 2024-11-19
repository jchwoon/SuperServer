# 📑 목차

- [프로젝트 개요](#-프로젝트-개요)
- [트러블 슈팅](#-트러블-슈팅)

# 👋 프로젝트 개요
* 📌 **장르 : 3D MMORPG**
* 📅 **2024. 09 ~ 진행중**
* 🔧 **기술 스택**
  * C#, .Net, Unity, EntityFrameworkCore, SQLServer, Protobuf
* ### [SuperWorld 시연영상](https://www.youtube.com/watch?v=Q42nrx8J2Wo)

# 👋 트러블 슈팅
1. 멀티 스레드 환경에서의 lock 문제
* 공용공간인 Room 안에서 로직을 lock을 잡고 수행
* 강제로 더미 클라이언트 수를 늘리고 Debugging 진행
* 메모리가 급격히 증거하고 수많은 Thread들이 생성되어 lock에 걸려있는 문제
* lock을 잡는 부분에선 비용이 많이 드는 로직을 수행하면 안 되겠다고 판단
* Command 패턴을 활용해 Thread Pool에서 제공된 Thread는 Queue에 일감(Action)
을 Push만 하도록 수행
* Queue에 쌓인 일감들은 Main Thread가 Queue에 일감이 쌓여있는지 체크하며 처
리
![image](https://github.com/user-attachments/assets/eabbdbbc-aaf1-4862-a1b7-72ec9885b473)

<br></br>
2. 이동 동기화
* Update마다 이동 패킷을 날리면 부하가 심할 거로 생각하여 일정 주기마다 패킷을 보
내야겠다고 판단
* 그러나 해당 주기 사이에 동기화가 안 되는 문제
* 다음 주기까지의 위치를 얘측해야겠다고 판단
* 위치와 rotY값을 받고 rotY와 단위원을 활용해 방향을 구함
* 오차가 클 경우를 대비해 Threshold 변수를 두어 범위를 벗어나면 실제 위치를
보내어 해결
<br></br>
3. 패킷 수신 과정에서 매번 새로운 byte array를 생성하는 문제
* 수신 시 맨 처음 한 번 큰 byte array를 생성해서 재사용해야겠다고 판단
* read Pos, write Pos를 만들고 ArraySegment를 활용해 소켓에서 들어온
byte 수만큼 쪼갠 후 write Pos 이동
* 쪼갠 ArraySegment를 읽어 들이고 읽어 들인 수만큼 readPos 이동
* 다음 수신을 할 때 readPos와 writePos가 같으면 0초기화 다르다면 아직 못 읽은
size만큼 처음 위치로 복사 (TCP이기에 가능)
<br></br>
4. 몬스터 AI에서 길 찾기를 해야 하는 문제
* 먼저 맵의 지형정보를 얻어야겠다고 판단
* 맵 위에 큰 Terrain을 하나 깔고 해당 Terrain의 위치를 활용하여 Raycast를
아래 방향으로 쏘고 갈 수 없는 지역을 binary파일로 저장하는 Tool을 만듦
* 서버에서 해당 파일과 A*, 그리고 PriorityQueue를 활용하여 해결
* A*는 8방향 탐색을 하며 PQ에서 우선순위는 (시작위치와 현재위치 간의 차이)
+ (목적지와 현재위치 간의 차이)가 작은걸로 우선순위를 결정



