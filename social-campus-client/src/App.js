import { useState } from "react";
import "./App.css";

function App() {
  const [count, setCount] = useState(0);

  function AddCount() {
    setCount(count + 1);
  }

  function SubCount() {
    setCount(count - 1);
  }

  return (
    <div className="App">
      <p>{count}</p>
      <button onClick={AddCount}>+</button>
      <button onClick={SubCount}>-</button>
    </div>
  );
}

export default App;
