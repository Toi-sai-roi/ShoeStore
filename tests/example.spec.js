const { test, expect } = require('@playwright/test');

// Đăng ký thành công - luồng chính
test('TC01 - Đăng ký thành công', async ({ page }) => {
    await page.goto('https://localhost:7165/Account/Register');

    await page.fill('[name=fullname]', 'Nguyễn Văn A');
    await page.fill('[name=email]', `test${Date.now()}@gmail.com`); // email random để không bị trùng
    await page.fill('[name=password]', '123456');
    await page.fill('[name=phone]', '0912345678');
    await page.fill('[name=address]', 'Hà Nội');

    await page.click('button[type=submit]');

    // Sau khi đăng ký thành công, redirect về trang chủ hoặc login
    await expect(page).toHaveURL(/\/(Account\/Login|$)/);
});

// Email đã tồn tại - luồng ngoại lệ
test('TC02 - Email đã tồn tại', async ({ page }) => {
    await page.goto('https://localhost:7165/Account/Register');

    await page.fill('[name=fullname]', 'Nguyễn Văn B');
    await page.fill('[name=email]', 'admin@shop.com'); // email đã có sẵn trong DB
    await page.fill('[name=password]', '123456');

    await page.click('button[type=submit]');

    // Phải ở lại trang Register và có thông báo lỗi
    await expect(page).toHaveURL(/Account\/Register/);
});

// Bỏ trống họ tên
test('TC03 - Bỏ trống họ tên', async ({ page }) => {
    await page.goto('https://localhost:7165/Account/Register');

    await page.fill('[name=email]', 'test@gmail.com');
    await page.fill('[name=password]', '123456');

    await page.click('button[type=submit]');

    const error = await page.textContent('#fullname-error');
    expect(error).toBe('Vui lòng nhập họ tên.');
});

// Mật khẩu dưới 6 ký tự
test('TC04 - Mật khẩu ngắn hơn 6 ký tự', async ({ page }) => {
    await page.goto('https://localhost:7165/Account/Register');

    await page.fill('[name=fullname]', 'Nguyễn Văn A');
    await page.fill('[name=email]', 'test@gmail.com');
    await page.fill('[name=password]', '123'); // chỉ 3 ký tự

    await page.click('button[type=submit]');

    const error = await page.textContent('#password-error');
    expect(error).toBe('Mật khẩu tối thiểu 6 ký tự.');
});

// Số điện thoại không hợp lệ
test('TC05 - Số điện thoại không hợp lệ', async ({ page }) => {
    await page.goto('https://localhost:7165/Account/Register');

    await page.fill('[name=fullname]', 'Nguyễn Văn A');
    await page.fill('[name=email]', 'test@gmail.com');
    await page.fill('[name=password]', '123456');
    await page.fill('[name=phone]', '012345678'); // sai định dạng

    await page.click('button[type=submit]');

    const error = await page.textContent('#phone-error');
    expect(error).toContain('Số điện thoại không hợp lệ');
});