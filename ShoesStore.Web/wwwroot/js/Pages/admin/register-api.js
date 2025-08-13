document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('registerForm');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const passwordMatchError = document.getElementById('password-match-error');
    const passwordStrengthBar = document.querySelector('#password-strength .progress-bar');
    const serverErrorDiv = document.getElementById('server-error');
    const successDiv = document.getElementById('success-message');
    const submitButton = document.getElementById('register-submit');

    // --- Password Strength Checker ---
    passwordInput.addEventListener('input', function () {
        const password = this.value;
        let strength = 0;
        if (password.length >= 8) strength += 25;
        if (password.match(/[a-z]/) && password.match(/[A-Z]/)) strength += 25;
        if (password.match(/\d/)) strength += 25;
        if (password.match(/[^a-zA-Z\d]/)) strength += 25;

        passwordStrengthBar.style.width = strength + '%';
        if (strength < 50) passwordStrengthBar.className = 'progress-bar bg-danger';
        else if (strength < 75) passwordStrengthBar.className = 'progress-bar bg-warning';
        else passwordStrengthBar.className = 'progress-bar bg-success';
    });

    // --- Confirm Password Checker ---
    confirmPasswordInput.addEventListener('input', function () {
        if (passwordInput.value !== this.value) {
            passwordMatchError.style.display = 'block';
        } else {
            passwordMatchError.style.display = 'none';
        }
    });

    // --- Form Submission ---
    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        serverErrorDiv.style.display = 'none';
        successDiv.style.display = 'none';

        if (passwordInput.value !== confirmPasswordInput.value) {
            passwordMatchError.style.display = 'block';
            return;
        }

        submitButton.disabled = true;
        submitButton.textContent = 'Đang xử lý...';

        const body = {
            email: emailInput.value,
            password: passwordInput.value,
        };

        try {
            const response = await fetch('/api/auth/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });

            if (response.ok) {
                form.style.display = 'none';
                successDiv.textContent = 'Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản của bạn.';
                successDiv.style.display = 'block';
            } else {
                const errorData = await response.json();
                // Lỗi từ ValidationProblem sẽ có cấu trúc `errors`
                const errorMessages = Object.values(errorData.errors || { general: [errorData.title] }).flat();
                serverErrorDiv.innerHTML = errorMessages.join('<br>');
                serverErrorDiv.style.display = 'block';
            }
        } catch (error) {
            serverErrorDiv.textContent = 'Đã có lỗi xảy ra. Vui lòng thử lại.';
            serverErrorDiv.style.display = 'block';
        } finally {
            submitButton.disabled = false;
            submitButton.textContent = 'Đăng Ký';
        }
    });
});