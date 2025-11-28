 cline-rules.md — Zarus (radicazz/zarus)

These rules define how **Cline** must work in this repo. They are aligned with the project’s existing guidance in `AGENTS.md`, `FEATURE.md`, and `README.md` and must be followed for every change.

## 0) Non-negotiables (read first)

- Use **MCP-first** workflows for Unity projects (Unity must be running for MCP workflows).:contentReference[oaicite:0]{index=0}
- Work inside `Assets/` to preserve GUIDs and references; **never hand-edit `.meta`** files.:contentReference[oaicite:1]{index=1}
- Avoid manual edits to Unity-managed folders (`Packages/`, `ProjectSettings/`, `UserSettings/`) unless unavoidable.:contentReference[oaicite:2]{index=2}
- Keep edits **incremental** and validate frequently.:contentReference[oaicite:3]{index=3}
- If `FEATURE.md` contains a plan for the work, follow it closely.:contentReference[oaicite:4]{index=4}

---

## 1) Commit-oriented design by default

Cline must treat every task as a **sequence of small, focused, reviewable commits**. This includes “design” changes (UI/UX, game balance, content, assets, architecture). Your default mode is:

1. **Propose a commit plan** (2–8 commits depending on scope).
2. Implement commit #1 fully, validate it, then move to commit #2, etc.
3. Keep history clean and atomic (one logical change per commit).:contentReference[oaicite:5]{index=5}

If the user asks for a large change, break it down into “commit-sized” slices and proceed sequentially.

---

## 2) Commit message conventions (mandatory)

Use the repo’s convention: `type: summary` with these common types: `feat`, `fix`, `docs`, `chore`, `tool`, `refactor`, `test`, `style`:contentReference[oaicite:6]{index=6}

Rules:
- One logical change per commit.:contentReference[oaicite:7]{index=7}
- Keep summaries short and concrete (imperative tone).
- Prefer this ordering when relevant:
  - `docs:` or `chore:` (prep / scaffolding)
  - `feat:` / `fix:` (implementation)
  - `test:` (validation)
  - `refactor:` (cleanup after behavior is correct)
- When following `FEATURE.md`, mirror its “commit-ordered plan” style: small, focused commits that preserve a clean history.:contentReference[oaicite:8]{index=8}

Examples:
- `feat: add cure outpost data model`
- `fix: prevent outpost build in fully infected provinces`
- `docs: update AGENTS onboarding for new HUD workflow`
- `refactor: simplify RegionMapController selection flow`

---

## 3) MCP workflow rules (how to edit safely)

Cline must prefer MCP-safe operations to preserve Unity asset integrity:

### Inspect before editing
- Use `read_resource` for scripts and text assets.
- Use `manage_scene` / `manage_gameobject` to inspect hierarchy and components.:contentReference[oaicite:9]{index=9}

### Edit safely (preferred tools)
- C# scripts: `script_apply_edits`:contentReference[oaicite:10]{index=10}
- UXML / USS: `manage_asset`:contentReference[oaicite:11]{index=11}
- Prefabs: `manage_prefabs`:contentReference[oaicite:12]{index=12}
- Use `apply_text_edits` only for small, precise changes.:contentReference[oaicite:13]{index=13}

### Validate after edits (every commit)
- Check console (`read_console`) and/or run `validate_script` after script changes.:contentReference[oaicite:14]{index=14}
- Run `run_tests` for Edit/PlayMode suites where applicable.:contentReference[oaicite:15]{index=15}

Hard rule:
- **Do not commit** if there are compilation errors or red console errors.

---

## 4) Unity testing requirement (must run in-editor)

After **each commit**, Cline must verify the change by running the project **in the Unity Editor**:

Minimum validation loop:
1. Ensure Unity is open for this project (Unity version: `6000.2.10f1`).:contentReference[oaicite:16]{index=16}
2. Open the relevant scene(s):
   - Core flow: Start → Main → End.:contentReference[oaicite:17]{index=17}
3. Press Play, reproduce the intended scenario, and confirm:
   - No console errors
   - The change behaves as designed
4. If the task affects GIS data (`Assets/Sprites/za.json`), run:
   - `Zarus/Map/Rebuild Region Assets` (instead of editing generated assets).:contentReference[oaicite:18]{index=18}

If tests exist for the touched area, run them before finalizing the commit.:contentReference[oaicite:19]{index=19}

---

## 5) Design changes: required discipline

When asked to make “design changes” (UI/UX, gameplay tuning, content structure, architecture), Cline must:

- Treat design work as **iterative commits** (scaffold → implement → refine).
- Keep each commit shippable: the project must still compile and run.
- Prefer data-driven, minimal scene surgery where possible (especially for gameplay features planned in `FEATURE.md`).:contentReference[oaicite:20]{index=20}

---

## 6) Documentation + onboarding updates

If you introduce a new workflow/pipeline, update onboarding docs:
- Document new pipelines directly in `AGENTS.md` to keep onboarding tight.:contentReference[oaicite:21]{index=21}

---

## 7) Final checklist (run per commit)

Before writing a commit message, confirm:

- [ ] I used MCP-safe tools for edits where applicable.:contentReference[oaicite:22]{index=22}
- [ ] I did not manually edit `.meta` files and stayed inside `Assets/` for asset work.:contentReference[oaicite:23]{index=23}
- [ ] Console is clean (no compile errors; no new red errors).:contentReference[oaicite:24]{index=24}
- [ ] I ran the game in-editor and verified expected behavior.
- [ ] Commit message matches `type: summary` and is a single logical change.:contentReference[oaicite:25]{index=25}

If any item fails, fix it before committing.