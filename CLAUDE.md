# CLAUDE.md — 통합 에이전트 작업 지침서

> 적용 대상: Claude / ChatGPT / Gemini 등 모든 AI 에이전트 공통 사용  
> 현재 활성 에이전트: **Claude (Claude Code)**

---

## 1. 에이전트 인식 및 문서 정책

- 현재 사용 중인 에이전트를 인식하여 해당 에이전트 규격에 맞게 작성한다.  
  (Claude → `CLAUDE.md`, 기타 에이전트도 동일 파일 참조 가능)
- 에이전트 문서는 **하나의 파일만 유지**한다.
- ChatGPT, Gemini 등 다른 에이전트에서도 활용 가능한 통합 문서로 관리한다.

---

## 2. 운영체제 환경

| 항목 | 값 |
|------|----|
| OS | Windows 11 Pro (10.0.26200) |
| Shell | PowerShell 5.1 |
| 아키텍처 | x64 |

> 이후 모든 명령어 및 스크립트는 **Windows / PowerShell** 기준으로 작성한다.

---

## 3. 프로젝트 정보

| 항목 | 값 |
|------|----|
| 프로젝트명 | C#_Macro |
| 워크스페이스 | `c:\Users\Administrator\Desktop\C#_Macro` |
| 언어 | **C# (.NET 8.0 / WinForms)** |
| 프로젝트 파일 | `Macro.csproj` |
| 빌드 출력 | `bin\Release\Publish\Macro.exe` (단일 파일) |
| 상태 | 개발 중 |

> 빌드는 **Visual Studio에서 Release 모드로 빌드**하거나 CLI에서 `dotnet build Macro.csproj -c Release --no-restore` 사용.

---

## 4. 워크플로우

```
작업 지시
  → 소스 수정 / 삭제 / 추가
  → 전체 소스 점검
  → 빌드
  → CLAUDE.md 및 README.md 업데이트
  → 작업 결과 기록
```

---

## 5. 문서 업데이트 정책

다음 경우에만 이 문서를 업데이트한다.

- 프로젝트 구조 변경
- 실행 방법 변경
- 환경 설정 변경

**단순 코드 수정은 문서에 기록하지 않는다.**  
반복된 오류는 기록하여 토큰 소모를 줄인다.

---

## 6. 언어별 환경 설정

### 6-1. C# (.NET 8.0)

- 빌드 방식: **Release 빌드** (Visual Studio 게시 기준)
- 출력 경로: `bin\Release\Publish\`
- 출력 형식: **단일 실행 파일 (Self-Contained Single File)**
- pdb 파일 생성 안 함

`.csproj` 필수 설정:

```xml
<PropertyGroup>
  <TargetFramework>net8.0-windows</TargetFramework>
  <OutputType>WinExe</OutputType>
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <PublishDir>bin\Release\Publish</PublishDir>
  <DebugType>none</DebugType>
  <DebugSymbols>false</DebugSymbols>
</PropertyGroup>
```

Visual Studio에서 빌드 시 자동 게시가 진행되도록 `.csproj`에 아래 타깃을 추가한다.

```xml
<Target Name="PublishAfterBuild" AfterTargets="Build" Condition="'$(Configuration)'=='Release'">
  <Exec Command="dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:DebugType=none /p:DebugSymbols=false -o bin\Release\Publish" />
</Target>
```

---

### 6-2. Python (conda 가상환경)

**Windows — `run.bat`**

```bat
@echo off
call conda init
call conda create -n C#_Macro python=3.9 -y
call conda activate C#_Macro

pip install [필요 패키지]

python main.py
```

> Linux / Mac 환경은 현재 OS가 Windows이므로 필요 시 추가한다.

---

## 7. 코드맵 (빌드 오류 추적)

빌드 오류 발생 시 코드맵을 생성하여 `CODEMAP.md`에 저장하고, 이후 작업에서 참고한다.

**코드맵 템플릿 — 실행 흐름 기록 형식**

```
1. 시작
2. 경로 / 환경 초기화
3. 출력 / 임시파일 환경 준비
4. 주요 처리 단계 (세부 내용 기술)
5. 결과 / 버전 계산
6. 정책 적용
7. 목록 갱신
8. 정리
9. 반환 객체 생성
10. 종료

※ 오류 발생 위치, 원인, 조치 내용을 각 단계 하위에 함께 기록한다.
```

---

## 8. 작업 완료 후 체크리스트

- [ ] 전체 소스 점검 완료
- [ ] 빌드 성공 확인
- [ ] `nul` 파일 존재 여부 확인 후 삭제 (Windows 환경)
- [ ] `CLAUDE.md` 업데이트 (구조·실행·환경 변경 시)
- [ ] `README.md` 업데이트

**`nul` 파일 삭제 명령 (PowerShell)**

```powershell
# Windows 예약 장치명 오탐을 피하기 위해 실제 파일만 검사
Get-ChildItem -Path . -Filter "nul" -Recurse -Force -File -ErrorAction SilentlyContinue | Remove-Item -Force
```

> 주의: `Get-ChildItem -Filter "nul"` 사용 시 Windows NUL 장치명이 모든 디렉터리에서 오탐된다. `-File` 플래그를 반드시 추가한다.

---

## 9. 표기 및 언어 정책

- 모든 작업 및 표기 사항은 **한국어**로 기록한다.
- `.md` 파일 또한 한국어로 작성한다.

---

## 변경 이력

| 날짜 | 변경 내용 | 구분 |
|------|-----------|------|
| 2026-03-16 | 최초 문서 작성 | 추가 |
| 2026-05-18 | 프로젝트 초기화 및 CLAUDE.md 생성 (신규 프로젝트, 언어 미정) | 추가 |
| 2026-05-18 | C# WinForms 매크로 프로그램 구현 완료 | 추가 |
| 2026-05-18 | .csproj AfterBuild 재귀 버그 수정 (`/p:IsPublishing=true` 가드 추가) | 수정 |
| 2026-05-18 | 기능 확장: 수동 추가(좌/우클릭, 키입력, 이동, 지연, 프로그램 실행/종료), 실행 키 사용자 설정, INI 저장, F7→위치 기록, 디자이너 파일 추가 | 추가 |
