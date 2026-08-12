import '../libs/css/App.css'
import Home from "./Dashboard/Home"
import Login from './Authorize/Login';
import { useState } from 'react';


function App() {
  const [userStatus, setUserStatus] = useState(() => {
  // Check localStorage, cookies, or auth context on initial mount
  return !!localStorage.getItem('authToken');
}); 

console.log(userStatus)

  
  return (
    <>
    {userStatus ? <Home /> : <Login />}
      
    </>
  )
}

export default App
