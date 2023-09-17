import { useState, useEffect } from "react";
import { Outlet, Link } from "react-router-dom";
import "./App.css";

function App() {
  const token = localStorage.getItem("token");
  const [data, setData] = useState({});
  const [city, setCity] = useState("");
  const [date, setDate] = useState("");
  function handleClick(e) {
    e.preventDefault();
    fetch(
      `https://localhost:7008/Solar/GetByName?cityName=${city}&date=${date}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    )
      .then((res) => res.json())
      .then((data) => {
        setData(data);
        console.log("Login response:", data);
      })
      .catch((error) => {
        console.error("Login error:", error);
      });
  }

  function formatDate(dateString) {
    const options = {
      month: "long",
      day: "numeric",
      hour: "numeric",
      minute: "numeric",
    };
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", options);
  }
  return (
    <>
      <div className="container">
        <div className="nav">
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </div>
        {token != null ? (
          <div>
            <h1>Wellcome!</h1>
            <p>Please enter a city name</p>
            <form className="form">
              <input
                type="text"
                onChange={(e) => setCity(e.target.value)}
                placeholder="city name"
              ></input>
              <input
                type="text"
                onChange={(e) => setDate(e.target.value)}
                placeholder="date"
              ></input>
              <button onClick={handleClick}>Search</button>
            </form>
          </div>
        ) : (
          <>
            <div>Please log in first!</div>
            <div className="subtitle">
              <Link to="/login">Login</Link>
            </div>
          </>
        )}
        {data && (
          <div>
            <h1>Response: </h1>
            <div>{city}</div>
            <div>Sunrise: {formatDate(data.sunrise)}</div>
            <div>Sunset: {formatDate(data.sunset)}</div>
          </div>
        )}
      </div>

      <div className="gradient"></div>
    </>
  );
}

export default App;
