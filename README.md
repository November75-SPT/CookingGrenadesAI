# This mod was sparked by the brilliant idea of 👑[adishee](https://hub.sp-tarkov.com/user/56229-adishee/). Heartfelt thanks to [adishee](https://hub.sp-tarkov.com/user/56229-adishee/) for their inspiration!

# CookingGrenadesAI

AI cooks grenades with varying probability and timing accuracy based on their level. This mod does **not** alter Big Brain or general AI behavior; When the AI pulls out a grenade to throw the grenade, we calculate the cooking probability and pauses the AI's grenade throw animation to simulate cooking.

Demonstration with perfect cooking timing.

![gif](README_Images/README_EXAMPLE.gif)

## Features
- **Level-Based Cooking**:
  - Low-level AI (Level 1): 10% cooking chance, ±0.3~0.5 timing error.
  - High-level AI (Max Level): 50% cooking chance, ±0~0.1 timing error.
- **Configurable Settings**:
  - Adjust cooking probability and timing error.
  - Enable/disable logging for debugging.
- **Testing Tools**:
  - Test cooking probability and timing error across levels and base times.

## How It Works

- **When QuickGrenadeThrowHandsController.Spawn (**`PatchPostfix`**)**:
  - Calculates cooking probability to decide if the AI should cook.
  - Estimates grenade flight time to the target.
  - Computes cooking time: grenade fuse time (`GetExplDelay`) minus flight time, adjusted for level-based error.
  - Pauses the throw animation (`Animator.speed = 0`) to simulate cooking.
  - Resumes animation after subtracting the throw animation time (~0.7842s)

- **Cooking Probability (**`ShouldCookGrenade`**)**:
  - Determines if AI cooks based on level.
  - Uses linear interpolation (`Mathf.Lerp`) between `MinLevelCookingProbability` (10% at level 1) and `MaxLevelCookingProbability` (50% at max level).

- **Flight Time Calculation (**`CalculateFlightTime`**)**:
  - Estimates grenade flight time from AI to target.
  - Calculates time using distance and speed, adding 12.5% per 10m to account for trajectory curve (designed with Grok AI's help).
  - **Note**: The flight time formula is not perfect, as it only considers straight-line distance with a basic curve adjustment.

- **Timing Error Adjustment (**`AdjustErrorCookingTime`**)**:
  - Adjusts cooking time with level-based error.
  - Interpolates error range: ±0.3\~0.5 (level 1) to ±0\~0.1 (max level).
  - Randomly speeds up or slows down cooking time.

## Note
- Due to my lack of coding skills, the AI animation pauses at the start rather than mid-animation.
- This mod does not affect accuracy, but the default vanilla AI throw accuracy is not perfect. Cooked grenades may hit walls and return to the AI.

## Installation
- Requires SPT 3.11.x
- Unzip into your SPT folder.

## Configuration
- **Cooking Probability**:
  - `Min Level Cooking Probability`: Chance at level 1 (default: 0.1, 10%). (Scav LVL is 1)
  - `Max Level Cooking Probability`: Chance at max level (default: 0.5, 50%). (Scav LVL is 1)
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


## Videio referance
0 Error Test

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/6XYw7iSjIJg/0.jpg)](https://www.youtube.com/watch?v=6XYw7iSjIJg)

Default Error Test

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/rq3GU__fayY/0.jpg)](https://www.youtube.com/watch?v=rq3GU__fayY)

## Credits
- **Idea From** : This mod was sparked by the brilliant idea of 👑[adishee](https://hub.sp-tarkov.com/user/56229-adishee/). Heartfelt thanks to ❤️[adishee](https://hub.sp-tarkov.com/user/56229-adishee/) for their inspiration!
- **Amazing .csproj Support**: Huge thanks to ❤️[Michael P. Starkweather](https://github.com/mpstark) and ❤️[CJ](https://github.com/CJ-SPT) for their fantastic .csproj files, which made this mod possible. Your help is truly appreciated!
- **SPT Modding Community**: I referenced the source code of many mods for learning purposes while creating this mod. Sincere thanks to the community for making their mods open-source and helping me learn.
- **Grok AI**: An all-knowing friend who answers any question I have.