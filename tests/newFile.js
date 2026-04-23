const { test, expect } = require('@playwright/test');

test('check homepage loads', async ({ page }) => {
    await page.goto('http://localhost:7165');
    const title = await page.title();
    console.log('Title:', title);
    expect(title).toBeTruthy();
});
