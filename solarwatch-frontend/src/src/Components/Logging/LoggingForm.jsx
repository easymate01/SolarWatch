import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Outlet, Link } from "react-router-dom";

//import "./LoggingStyle.css";

const LoggingForm = ({ isHandleRegister, isLogin }) => {
  const [saveUsername, setSaveUsername] = useState("");
  const [saveEmail, setSaveEmail] = useState("");
  const [savePassword, setSavePassword] = useState("");
  const navigate = useNavigate();
  

  const handleRegister = (e) => {
    e.preventDefault();
    console.log("Registering...");
    fetch(`https://localhost:7148/Auth/Register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        username: saveUsername,
        email: saveEmail,
        password: savePassword,
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log("Registration response:", data);
        navigate("/");
      })
      .catch((error) => {
        console.error("Registration error:", error);
      });
  };

  const handleLogin = (e) => {
    e.preventDefault();
    console.log("Logging in...");
    fetch(`https://localhost:7148/Auth/Login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: saveEmail,
        password: savePassword,
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log("Login response:", data);
        if (data.token) {
            // Save the token to local storage or a state variable, depending on your application's architecture
            localStorage.setItem("token", data.token);
          }
        navigate("/");
      })
      .catch((error) => {
        console.error("Login error:", error);
      });
  };
  return (
    <>
      <div className="form">
        {!isHandleRegister ? (
          <>
            <div className="title2">Please Log In!</div>
            <div className="subtitle"></div>
          </>
        ) : (
          <>
            <div className="title2">Welcome</div>
            <div className="subtitle">Let's create your account!</div>
          </>
        )}
        <div className="input-container ic1">
          <input
            id="firstname"
            className="input"
            type="text"
            placeholder=" "
            onChange={(e) => setSaveUsername(e.target.value)}
          />
          <div className="cut"></div>
          <label htmlFor="email" className="placeholder">
            Username
          </label>
        </div>

        <div className="input-container ic2">
          <input
            id="email"
            className="input"
            type="text"
            placeholder=" "
            onChange={(e) => setSaveEmail(e.target.value)}
          />
          <div className="cut cut-short"></div>
          <label htmlFor="email" className="placeholder">
            Email
          </label>
        </div>

        <div className="input-container ic2">
          <input
            id="lastname"
            className="input"
            type="password"
            placeholder=" "
            onChange={(e) => setSavePassword(e.target.value)}
          />
          <div className="cut"></div>
          <label htmlFor="lastname" className="placeholder">
            Password
          </label>
        </div>
        {isHandleRegister ? (
          <>
            <button type="text" className="submit" onClick={handleRegister}>
              Register
            </button>
            <div className="subtitle">
              <Link to="/login">Login</Link>
            </div>
          </>
        ) : (
          <>
            <button type="text" className="submit" onClick={handleLogin}>
              Login
            </button>
            <div className="subtitle">
              <Link to="/register">Register</Link>
            </div>
          </>
        )}
      </div>
    </>
  );
};

export default LoggingForm;