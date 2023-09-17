import { useState, useEffect } from "react";
import { Outlet, Link } from "react-router-dom";
import "./App.css";

function App() {
  const token = localStorage.getItem("token");
  const [step, setStep] = useState(1);
  const [data, setData] = useState({});
  const [city, setCity] = useState("");
  const [date, setDate] = useState("");
  const [loading, setLoading] = useState(true);
  const [showRes, setShowRes] = useState(false);
  const [deleted, setDeleted] = useState(false);

  const nextStep = () => {
    setStep(step + 1);
    setShowRes(false);
  };

  const prevStep = () => {
    setStep(step - 1);
    setShowRes(false);
  };

  function handleSubmit(e) {
    e.preventDefault();
    setShowRes(true);
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
        setLoading(false);
      })
      .catch((error) => {
        console.error("Login error:", error);
      });
  }

  const handleDelete = (e) => {
    e.preventDefault();
    fetch(`https://localhost:7008/Solar/DeleteByName?cityName=${city}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`City deletion failed with status: ${res.status}`);
        }
        return res.text(); // Parse response as text
      })
      .then((textResponse) => {
        console.log("Delete response:", textResponse);
        setDeleted(true);
        setData([]);
        setStep(1);
        setCity("");
        setDate("");
        setShowRes(false);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Delete error:", error);
      });
  };

  function formatDate(dateString) {
    if (!dateString) {
      return " ";
    }

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
          <Link to="/">Home</Link>

          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </div>
        {token != null ? (
          <div>
            <h1>Wellcome!</h1>
            <form className="form">
              {step === 1 && (
                <>
                  <p>1. Please enter a city name</p>

                  <input
                    type="text"
                    onChange={(e) => {
                      setCity(e.target.value);
                      setDeleted(false);
                    }}
                    placeholder="city name"
                    value={city}
                  />
                  <button type="button" onClick={nextStep}>
                    Next
                  </button>
                </>
              )}
              {step === 2 && (
                <>
                  <p>2. Please enter a date</p>

                  <input
                    type="text"
                    onChange={(e) => setDate(e.target.value)}
                    placeholder="date"
                    value={date}
                  />
                  <button type="button" onClick={prevStep}>
                    Previous
                  </button>
                  <button type="submit" onClick={handleSubmit}>
                    Search
                  </button>
                </>
              )}
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
        {showRes && (
          <div className="response">
            {loading ? (
              <div>Loading...</div>
            ) : (
              <>
                <h1>Response: </h1>
                <div>City: {city}</div>
                <div>Sunrise: {formatDate(data.sunrise)}</div>
                <div>Sunset: {formatDate(data.sunset)}</div>
                <button className="small-btn">Edit city</button>
                <button className="small-btn" onClick={handleDelete}>
                  Delete city
                </button>
              </>
            )}
          </div>
        )}
      </div>
      {deleted && <div>City Deleted successfully ✅</div>}
      <div className="gradient"></div>
    </>
  );
}

export default App;
