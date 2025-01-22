import axios from "axios";

export default axios.create({
  baseURL: "http://localhost:5169",
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});
