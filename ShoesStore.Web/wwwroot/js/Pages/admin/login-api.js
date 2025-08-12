// anti-forgery cho POST
function getAfToken() { return document.querySelector('input[name="__RequestVerificationToken"]').value; }

document.getElementById('adminLoginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('email').value?.trim();
    const password = document.getElementById('password').value;
    const rememberMe = document.getElementById('remember').checked;

    let recaptchaToken = "";
    const siteKey = "@siteKey";
    if (siteKey) {
        try { recaptchaToken = await grecaptcha.execute(siteKey, { action: 'login' }); }
        catch { /* im lặng, không chặn login nếu recaptcha lỗi tải */ }
    }

    const body = {
        email, password, rememberMe,
        recaptchaToken // server có thể bỏ qua nếu bạn chưa bật verify
    };

    // Identity API endpoints .NET 8:
    // /api/auth/login?useCookies=true&useSessionCookies=true
    // -> set cookie đăng nhập; rememberMe sẽ quyết định persistent cookie.
    const resp = await fetch('/api/auth/login?useCookies=true&useSessionCookies=true', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAfToken()
        },
        credentials: 'include',
        body: JSON.stringify(body)
    });

    if (resp.ok) {
        location.href = '/Index';
    }
    else {
        const data = await resp.json().catch(() => ({}));
        let msg = data.detail || data.message || 'Đăng nhập thất bại';

        // Logic suy đoán từ phản hồi mặc định
        if (msg.includes("locked out")) {
            msg = "Tài khoản đã bị khóa do đăng nhập sai nhiều lần.";
        } else if (msg.includes("Invalid username or password")) {
            msg = "Email hoặc mật khẩu không chính xác.";
        } else if (msg.includes("confirm your email")) {
            msg = "Vui lòng xác nhận email trước khi đăng nhập.";
        }

        const $err = document.getElementById('err');
        $err.textContent = msg;
        $err.style.display = 'block';
        setTimeout(() => { $err.style.display = 'none'; }, 5000);
    }
});