# This mod was inspired by an idea from [adishee](https://hub.sp-tarkov.com/user/56229-adishee/). Many thanks to [adishee](https://hub.sp-tarkov.com/user/56229-adishee/).

# CookingGrenadesAI

AI cooks grenades with varying probability and timing accuracy based on their level. This mod does **not** alter Big Brain or general AI behavior; it pauses the AI's grenade throw animation to simulate cooking.

## Features
- **Level-Based Cooking**:
  - Low-level AI (Level 1): 10% cooking chance, ±0.3~0.5 timing error.
  - High-level AI (Max Level): 50% cooking chance, ±0~0.1 timing error.
- **Configurable Settings**:
  - Adjust cooking probability and timing error.
  - Enable/disable logging for debugging.
- **Testing Tools**:
  - Test cooking probability and timing error across levels and base times.

## Note
- Due to my lack of coding skills, the AI animation pauses at the start rather than mid-animation.
- AI throw accuracy is not perfect. Cooked grenades may hit walls and return to the AI.

## Installation
- Unzip into your SPT folder.

## Configuration
- **Cooking Probability**:
  - `Min Level Cooking Probability`: Chance at level 1 (default: 0.1, 10%).
  - `Max Level Cooking Probability`: Chance at max level (default: 0.5, 50%).
- **Timing Error**:
  - `Min/Max Error at Low Level`: Error range at level 1 (default: 0.3~0.5).
  - `Min/Max Error at High Level`: Error range at max level (default: 0~0.1).
- **Testing**:
  - `Test Level`: Level to test (0 = all levels, default: 0).
  - `Test Base Time`: Base time to test (0 = 0.5 to 5.0 step 0.5, default: 0).
  - `Enable Cooking Probability Test`: Enable probability test (default: false).
  - `Enable Timing Error Test`: Enable error test (default: false).
- **Logging**:
  - `Log to File`: Enable logging to `BepInEx/LogOutput.log` (default: false).
  - `Log to In-Game Debug Window`: Enable in-game debug logs (default: false).

## Testing

Use the testing to verify AI behavior:

1. Enable tests in `Enable Cooking Probability Test` or `Enable Timing Error Test`.
   - Configure `Test Level` (e.g., 1 for level 1, 0 for all).
   - Configure `Test Base Time` (e.g., 2.0 for 2s, 0 for 0.5~5.0).
2. Run the game and check logs:
   - `BepInEx/LogOutput.log` (if `Log to File` enabled).
   - In-game debug window (if `Log to In-Game Debug Window` enabled).


## Debugging

Enable logging:
- `Log to File`: Outputs to `BepInEx/LogOutput.log`.
- `Log to In-Game Debug Window`: Shows logs in-game.

Logs include:
- AI cooking attempts (`LVL X: Chance Y, Random Z`).
- Timing errors (`ErrorRange X~Y => Z`).
- Test results.

## Credits
- **Idea From** : This mod was inspired by an idea from ❤️[adishee](https://hub.sp-tarkov.com/user/56229-adishee/). Many thanks to ❤️[adishee](https://hub.sp-tarkov.com/user/56229-adishee/).
- **Amazing .csproj Support**: Huge thanks to ❤️[Michael P. Starkweather](https://github.com/mpstark) and ❤️[CJ](https://github.com/CJ-SPT) for their fantastic .csproj files, which made this mod possible. Your help is truly appreciated!
- **SPT Modding Community**: I referenced the source code of many mods for learning purposes while creating this mod. Sincere thanks to the community for making their mods open-source and helping me learn.
- **Grok AI**: An all-knowing friend who answers any question I have.