import axios from "axios";

export default axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "https://localhost:8081",
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});
