# Change plan

> Draft. This file tracks what changed on the way to this repo and what I plan to change next. Edit freely.

## Origin

Originally developed at Bigpoint. Published here with Bigpoint's permission for research, education and other noncommercial use. Copyright (c) 2026 Bigpoint; see [LICENSE.md](LICENSE.md).

## Changes made before publishing

- Namespaces are now `TeaSpoons.*` and the package id is `com.tea-spoons.simple-localization` (assemblies renamed to match).
- Internal build, registry and tracker references were removed; the repo uses GitHub Actions (`CI` and `Release`) built on `unity-ci-kit`.
- Added `LICENSE.md` (PolyForm Noncommercial 1.0.0), an install section in the README, and package metadata (author, license and documentation URLs).

## Planned changes

- [x] Tag and publish `v0.5.0` with the Release workflow.
- [ ] Run this package's tests in CI with `unity-ci-kit` (needs a small test-project helper in the kit).
<!-- review-items:start -->
- [ ] **P1** Decide the future and write it down: keep this package as it is, or deprecate it in favor of `localizer` with a migration guide. Do not merge code between the two (different owners and licenses).
- [ ] **P1** Fix the leak: an owner-bound helper (`Bind(MonoBehaviour owner)`) that disposes with the owner, plus a note in the README about `Dispose`.
- [ ] **P1** Declares `unity: 2022.3`, but only Unity 6000.3.8f1 was tested. Add a Unity version matrix to CI once package tests run there (see the `unity-ci-kit` plan), or raise the minimum.
- [ ] **P2** If the package stays, add plural forms and a regional fallback chain.
- [ ] **P2** Add a `CHANGELOG.md`. Unity's package layout lists one next to `README.md`, and the `unity-ci-kit` validator warns without it.
<!-- review-items:end -->

<!-- review:start -->
## Review (September 2026)

Reviewed as a senior Unity engineer would: I read the code and compared the package with similar open-source projects (September 2026). Those projects are listed for ideas only. Nothing was copied from them, and their licenses are noted in case code is ever reused. Priorities: **P0** correctness bug or broken metadata, **P1** should be done soon, **P2** nice to have.

### Compared with

| Project | License | Worth noting |
|---|---|---|
| [Unity Localization: Smart Strings](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/Smart/SmartStrings.html) | Unity package | Named placeholders, plurals chosen by the locale of the table, conditionals, lists and nesting, plus custom formatters. |
| [Unity Localization: overview](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html) | Unity package | String and asset tables, pseudo-localization for testing, and XLIFF, CSV and Google Sheets import and export. |
| [jpyankel/Localization-Manager, Mukarillo/UnityLocalizationManager](https://github.com/jpyankel/Localization-Manager) | not checked | Small JSON and CSV based managers, similar in scope to this package. |

Licensing caution: this package is Bigpoint's code and `localizer` is my own MIT code. Do not copy code between them. A migration guide is fine; a merge is not.

### Findings from reading the code

- **[Leak]** `Localization.LanguageUpdated` is a static event. `AutoTranslation` subscribes to it when `UpdateOnLanguageChange` is on and unsubscribes only in `Dispose`. A forgotten `Dispose` keeps the object, and whatever UI its callback captured, alive for the whole session.
- **[Gap]** No plural forms and no regional fallback (regional to language to default). Placeholders are named, through `LocalizedStringFormatting`.
- **[Overlap]** The new, MIT-licensed `localizer` package has plurals, fallback and JSON tables. Two localization packages in one organization will confuse users.
<!-- review:end -->

## Notes and ideas

_Add your own here._
