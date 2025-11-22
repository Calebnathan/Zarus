# UI System Setup Guide

This guide walks you through setting up the UI system in Unity Editor.

## Prerequisites

- Unity 6000.2.10f1
- MCP for Unity package installed
- Assets folder structure created (already done)

## Setup Steps

### 1. Create PanelSettings Assets

PanelSettings define how UI renders on screen.

**For Screen Space Overlay (HUD, Menus):**
1. Right-click in `Assets/UI/Settings/`
2. Create → UI Toolkit → Panel Settings Asset
3. Name it `PanelSettings_Overlay`
4. Configure:
   - **Target Display**: Display 1
   - **Screen Space**: Check this
   - **Sort Order**: 0
   - **Scale Mode**: Constant Pixel Size
   - **Reference Resolution**: 1920 × 1080

**For World Space (Diegetic UI - Future):**
1. Create another Panel Settings Asset
2. Name it `PanelSettings_WorldSpace`
3. Configure:
   - **Uncheck Screen Space**
   - Set render camera reference in scenes

### 2. Set Up UI GameObjects in Main Scene

**Create UIManager GameObject:**
1. In Main scene, create empty GameObject
2. Name it `UIManager`
3. Add `UIManager` component
4. Reference InputActions asset in Inspector:
   - Drag `Assets/InputSystem_Actions.inputactions` to Input Actions field

**Create PauseMenu GameObject:**
1. Create empty GameObject under UIManager
2. Name it `PauseMenu`
3. Add `UIDocument` component:
   - **Panel Settings**: Select `PanelSettings_Overlay`
   - **Source Asset**: Drag `Assets/UI/Layouts/Screens/PauseMenu.uxml`
4. Add `PauseMenu` script component
5. The UIDocument will auto-reference

**Create GameHUD GameObject:**
1. Create empty GameObject under UIManager
2. Name it `GameHUD`
3. Add `UIDocument` component:
   - **Panel Settings**: Select `PanelSettings_Overlay`
   - **Source Asset**: Drag `Assets/UI/Layouts/Screens/GameHUD.uxml`
4. Add `GameHUD` script component
5. In Inspector, reference:
   - **Map Controller**: Drag RegionMapController from scene

**Link to UIManager:**
1. Select UIManager GameObject
2. In UIManager component:
   - **Pause Menu**: Drag PauseMenu GameObject
   - **Game HUD**: Drag GameHUD GameObject

### 3. Apply Theme Stylesheet

For each UIDocument:
1. Select the GameObject (PauseMenu or GameHUD)
2. In UIDocument component, expand **Style Sheets**
3. Add new stylesheet entry
4. Drag `Assets/UI/Styles/MainTheme.uss`

### 4. Test the System

**Play Mode Test:**
1. Enter Play Mode
2. HUD should appear showing:
   - Timer starting at 00:00
   - Provinces: 0 / 9
3. Press ESC → Pause menu should appear
4. Click Resume or press ESC again → Should resume
5. Hover over provinces → Info should update in bottom bar
6. Click provinces → Visit counter should increment

## Troubleshooting

### UI Not Appearing

**Check:**
- UIDocument has PanelSettings assigned
- UXML file is referenced in UIDocument
- MainTheme.uss is in StyleSheets list
- GameObject is active in hierarchy

### Pause Not Working

**Check:**
- InputActionAsset is assigned in UIManager
- "UI/Cancel" action exists in InputSystem_Actions
- Player and UI action maps exist

### Timer Not Updating

**Check:**
- Time.timeScale is not 0 when game starts
- GameHUD script is attached
- Update() is being called (check with Debug.Log)

### Province Info Not Showing

**Check:**
- RegionMapController reference in GameHUD
- Events are firing (add Debug.Log to OnProvinceHovered)
- UXML element names match code exactly

## Next Steps

After basic setup works:

1. **Add Province Detail Screen** (diegetic card)
   - Create ProvinceDetailCard.uxml
   - Show stats, description, image
   - Animate in from side

2. **Add Settings Screen**
   - Graphics, Audio, Controls tabs
   - Save/Load preferences

3. **Add Confirmation Popups**
   - "Are you sure you want to quit?"
   - Reusable dialog component

4. **Enhance Animations**
   - Fade transitions
   - Slide-in effects
   - Button hover effects

5. **Add Sound Effects**
   - Button clicks
   - Menu open/close
   - Province selection

## Visual Customization

Artists can modify colors without code:

1. Open `Assets/UI/Styles/MainTheme.uss`
2. Find `:root` section
3. Change color values:
   ```css
   --color-primary: rgb(27, 151, 174);    /* Your color here */
   --bg-dark: rgba(15, 20, 30, 0.95);     /* Background color */
   ```
4. Save file
5. Changes apply immediately in Editor

## Performance Notes

- UI Toolkit uses batched rendering (very efficient)
- Minimize `display: none` → `display: flex` changes
- Use object pooling for frequently created cards
- Profile with Unity Profiler → UI module

## Resources

- Full README: `Assets/UI/README.md`
- Unity Docs: https://docs.unity3d.com/Manual/UIElements.html
- USS Syntax: Similar to CSS3
