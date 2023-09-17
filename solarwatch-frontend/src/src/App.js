import { useState, useEffect } from 'react';
import './App.css';


function App() {
  const token = localStorage.getItem("token");
  const [data, setData] = useState({});
  const [city, setCity] = useState("");

  function handleClick(e){
    e.preventDefault();

    fetch(`https://localhost:7148/SolarWatch/GetByName?cityName=${city}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      }
    })
      .then((res) => res.json())
      .then((data) => {
        setData(data);
        console.log("Login response:", data);
      })
      .catch((error) => {
        console.error("Login error:", error);
      });
  }

   


  return (
   <div>
    {token != null ? (<div>
      <input type="text" onChange={(e) => setCity(e.target.value)}></input>
      <button onClick={handleClick}>Search</button>
    </div>) : (<div>Please log in first!</div>)}
  {data && (<div>{data.lat}</div>)}
    </div>
  );
}

export default App;
