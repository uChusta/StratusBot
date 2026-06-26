# STratusBot / Stratus Shield — Cybersecurity Awareness Chatbot

## Project Overview
StratusBot (branded in the UI as "Stratus Shield") is a cybersecurity awareness chatbot with a Windows desktop GUI built using WPF and .NET 8. It is designed to educate users about cybersecurity topics through interactive chat, scenario-based learning, quizzes, and task management. The app focuses on approachable explanations, short exercises, and lightweight persistence to personalize interactions.

## Key Features
- Interactive conversation with a keyword-driven knowledge base.
- Knowledge base with extensive cybersecurity tips and FAQ-style answers.
- Scenario-based learning and contextual follow-ups.
- Security Awareness Quiz: full quiz UI with categorized questions, randomization, scoring, feedback, and quit handling.
- Tasks management: create, view, complete, and delete tasks via a TaskAssistant UI and chat commands.
- Activity logging: JSON-based ActivityLog with entries for user actions and events.
- Memory & persistence: MemoryStore persists user data (e.g., username, favorite topic) to AppData (JSON).
- Intent detection: phrase matching for intents (tasks, reminders, quizzes, viewing logs).
- UI/UX improvements:
  - Modern chat styling (message bubbles, rounded input, improved alignment).
  - Tabbed main window (Chat / Tasks).
  - User display & online/offline status indicator.
  - ASCII header branding ("Stratus Shield") and improved ASCII art alignment.
  - Welcome sound playback (NAudio integration) and improved audio handling.
- Robust fallback responses and sentiment-aware openers (sentiment detection, including "Sad").
- Developer-friendly modular architecture: ChatBot, KeywordResponder, SentimentDetector, MemoryStore, TaskManager, QuizManager, ActivityLog, TaskStorageHelper.

## Changelog — major items added since README update on 2026-06-03
- Add Security Awareness Quiz: QuizWindow UI, QuizManager, categorized and randomized questions, scoring and feedback. (commit: Add Security Awareness Quiz feature — https://github.com/uChusta/StratusBot/commit/b2b06035951b9d9f96e4203bb1fae1cd8d3f3872)
- Enhance quiz & tasks: quiz state tracking, answer recognition, quit handling, QuizStartRequested event, UI wiring to open QuizWindow, Exit Quiz confirmation. (commit: Enhance quiz and task features — https://github.com/uChusta/StratusBot/commit/7b27bb3693181d57aa62bce5514d2e709bb04c31)
- Tasks & TaskAssistant: TaskAssistant.xaml user control, TaskManager logic, and TaskStorageHelper with Cybertask model using JSON storage (Newtonsoft.Json). (commits: TaskAssistant & TaskManager — https://github.com/uChusta/StratusBot/commit/71ec72221894fd89b709c5508970e62335c07fab ; TaskStorageHelper — https://github.com/uChusta/StratusBot/commit/064c2544664c96427c39d95565316080b358525f)
- Activity logging & log management: refactor Activitylog → ActivityLog, add ActivityLog.cs and LogEntry structure for JSON logs. (commit: Refactor logging / ActivityLog — https://github.com/uChusta/StratusBot/commit/0cc211507de9cf87c777055628cdf4da4a08d3c0)
- Intent detection & activity tracking: add phrase-based intent detection for tasks, reminders, quizzes, and log viewing; add methods to track/display activities and format logs. (commit: Enhance chatbot with intent detection and activity logging — https://github.com/uChusta/StratusBot/commit/01cda70c864c8af9c956ab88386420fbb5736e9a)
- Tabbed main window: TabControl with separate Chat and Tasks tabs; TaskAssistant embedded into Tasks tab. (commit: Add TabControl Chat/Tasks — https://github.com/uChusta/StratusBot/commit/b8a60dc584d853d662d41ca8882f43e6f4b037ce)
- UI styling improvements: modern chat bubbles, rounded corners, updated ASCII header, centered ASCII formatting, auto-scroll behavior, improved input handling and focus. (commits: UI styling & ascii alignment — https://github.com/uChusta/StratusBot/commit/c6585387b697a388d2d9f3019ef01f491aacb626 ; ASCII centering cleanup — https://github.com/uChusta/StratusBot/commit/577562539a2669df1d3fc47e32b57b2c8ab6813c)
- Branding & sound: header changed to "Stratus Shield", improved fallback phrasing, play welcome sound on startup, tightened audio playback handling. (commit: UI branding, fallback, and sound — https://github.com/uChusta/StratusBot/commit/003b9ce55b4b53ab726ce9c05aec07dcd5de2f7b)
- Persistence & MemoryStore: persist user data (username, favorite topic) to AppData, expose MemoryStore for ChatBot usage, use thread-safe ConcurrentDictionary changes. (commits: Persistence and MemoryStore updates — https://github.com/uChusta/StratusBot/commit/7925aaf5e71bf1f2e3b936c53464b8b6d9779236 ; MemoryStore refactor — https://github.com/uChusta/StratusBot/commit/dac60b1a36177279ae542e9a1c9eccc16c533ac5)
- KeywordResponder & content expansion: large expansion of cybersecurity responses, greeting handling, and GetAllKeywords listing. (commits: Expand responses & keywords — https://github.com/uChusta/StratusBot/commit/1f3b8a5af15f0316c4110da640ff09069afdafb4 ; Expand tips — https://github.com/uChusta/StratusBot/commit/5dec677a4ed54c5516b4d557061ae3ea46b079eb)
- GitHub Actions / CI:
  - Add .NET Core Desktop workflow to build/test/package WPF app. (commit: Add GitHub Actions workflow — https://github.com/uChusta/StratusBot/commit/6d8c87db8092124ed8d9c0e2135a6a7d00659966)
  - Add changelog auto-generation workflow and follow-up workflow tweaks. (commits: Add changelog workflow — https://github.com/uChusta/StratusBot/commit/5ff39216edc40a60a600bc6472d3054f9f66f90f ; Update changelog workflow — https://github.com/uChusta/StratusBot/commit/d54281b52e2fb7dfeca049ad988fb517d198cace)
  - Some signing / artifact steps commented out to simplify the workflow while iterating. (commit: Comment out pfx handling — https://github.com/uChusta/StratusBot/commit/83a3bff854021fd77c5fd647b5aea4e1b2ff882b)

## How to build & run (developer notes)
- Requirements: Windows, .NET 8 SDK, Visual Studio (recommended) or dotnet CLI for WPF apps.
- Restore NuGet packages (project uses Newtonsoft.Json and NAudio).
- Open solution in Visual Studio and run the WPF project, or use `dotnet run` from project folder when supported.
- Data and logs:
  - MemoryStore persists small user state to the user's AppData in JSON.
  - Tasks are stored via TaskStorageHelper in JSON (Newtonsoft.Json).
  - ActivityLog writes JSON-formatted entries for actions/events.

## Developer notes & architecture
- Core components:
  - ChatBot: bot controller that uses KeywordResponder, SentimentDetector, MemoryStore and coordinates features.
  - KeywordResponder: keyword/phrase matched answers and topics.
  - SentimentDetector: lightweight sentiment enum detection to shape responses.
  - MemoryStore: thread-safe storage for user state.
  - TaskManager / TaskStorageHelper: task CRUD and persistence.
  - QuizManager / QuizWindow: quiz lifecycle and UI.
  - ActivityLog: structured logging & log management for audit/viewing.
- UI:
  - MainWindow contains a TabControl (Chat / Tasks).
  - TaskAssistant is a reusable UserControl for tasks.

## Next steps / TODOs (ideas)
- Add localization support for multi-language tips.
- Add unit tests for QuizManager and TaskManager logic.
- Add secure storage options for sensitive config (avoid storing secrets in repo / plain JSON).
- Improve installer packaging and sign build artifacts when ready.

---
Updated README: document features and changelog since 2026-06-03
