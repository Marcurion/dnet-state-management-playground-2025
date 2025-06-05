Combining **manual component rendering in Blazor** with **CSS animations** is totally doable and gives you precise control over **when** the component re-renders and **how** elements animate.

---

## ✅ Goal

* **Render UI only when you want** (`StateHasChanged` + `ShouldRender`)
* **Trigger CSS animations** manually when state changes
* Avoid unnecessary re-renders that break or restart animations

---

## 🧱 Approach Overview

1. **Trigger `StateHasChanged()` manually** when view state changes.
2. **Apply or toggle a CSS class** to activate a CSS animation.
3. Use **CSS animations** or **transitions** (no JS required).
4. Optionally, **avoid animation flicker** by using `@keyframes` with `animation-fill-mode`.

---

## 🖼️ Example: Fading in a Message on State Update

### 🔹 CSS (in `site.css` or inline `<style>`)

```css
.fade-in {
    animation: fadeIn 0.5s ease-in-out;
    animation-fill-mode: both;
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to   { opacity: 1; transform: translateY(0); }
}
```

---

### 🔹 Blazor Component (`MyComponent.razor`)

```razor
<div class="@animationClass">
    @viewState.Message
</div>

<button @onclick="TriggerUpdate">Update</button>

@code {
    private MyViewState viewState = new();
    private string animationClass = "";

    private bool allowRender = false;

    protected override bool ShouldRender() => allowRender;

    private void TriggerUpdate()
    {
        viewState.Message = "Updated at " + DateTime.Now;

        animationClass = ""; // Reset class to retrigger animation
        allowRender = true;
        StateHasChanged();
        allowRender = false;

        // Delay applying class until next render frame
        _ = Task.Run(async () =>
        {
            await Task.Delay(10); // small delay ensures reflow
            animationClass = "fade-in";
            await InvokeAsync(StateHasChanged);
        });
    }
}
```

---

## 🧠 Why It Works

* You **control when Blazor re-renders** via `StateHasChanged` and `ShouldRender`.
* You **manually add the animation class** after the re-render to let the browser reflow the DOM.
* The short `Task.Delay(10)` ensures the DOM applies the class after rendering, allowing the animation to run.

---

## 🔄 Bonus: Restart Animation on Repeated Updates

To restart the same animation every time:

1. Clear the class (`animationClass = ""`)
2. Wait briefly (`Task.Delay`)
3. Reapply the class (`animationClass = "fade-in"`)

This ensures CSS restarts the animation on each update.

---

## ✅ Summary

| Feature                    | Solution                                   |
| -------------------------- | ------------------------------------------ |
| Manual render control      | Use `ShouldRender()` + `StateHasChanged()` |
| CSS animation trigger      | Apply/toggle CSS class                     |
| Prevent animation flicker  | Use `animation-fill-mode: both`            |
| Restart animation manually | Clear/reapply class with small delay       |

---

Let me know if you want this applied to list items, modal components, or transition groups — I can provide scoped animation strategies for those too.
