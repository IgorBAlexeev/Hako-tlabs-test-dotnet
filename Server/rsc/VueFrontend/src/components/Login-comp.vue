<template>
  <form @submit.prevent="handleLogin" class="login-form">
    <h2>Login</h2>
    <div class="form-group">
      <label for="username">Username:</label>
      <input type="text" id="username" v-model="username" required />
    </div>
    <div class="form-group">
      <label for="password">Password:</label>
      <input type="password" id="password" v-model="password" required />
    </div>
    <button type="submit">Log In</button>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const username = ref('');
const password = ref('');
const router = useRouter();
const handleLogin = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: username.value,
          password: password.value,
        }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Login failed');
      }
      const data = await response.json();
      const { token, user } = data;

      localStorage.setItem('userToken', token);
      localStorage.setItem('userData', JSON.stringify(user));
      router.go(-1);
    } catch (error) {
      console.error('Login error:', error);
    } finally {
      //
    }
  };
</script>

<style scoped>
.login-form {
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-width: 300px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.form-group {
  display: flex;
  flex-direction: column;
}
.error-message {
  color: red;
}
</style>
