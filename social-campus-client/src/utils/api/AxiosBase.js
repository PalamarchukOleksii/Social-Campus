import axios from "axios";

export default axios.create({
  baseURL: "https://localhost:7106",
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});
