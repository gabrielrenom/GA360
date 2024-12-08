import DuendeContext from "contexts/DuendeContext";
import { useContext } from "react";

const AuthZTestComponent = () => {
    const { user, isLoggedIn } = useContext(DuendeContext);
  
    if (!isLoggedIn) {
        return <div>Please log in to view your details.</div>;
    }
  
    return (
        <div>
            <h1>Welcome, {user.name}</h1>
            <p><strong>Email:</strong> {user.email}</p>
            <p><strong>ID:</strong> {user.id}</p>
            <p><strong>Role:</strong> {user.role}</p>
            <p><strong>Role ID:</strong> {user.roleId}</p>
        </div>
    );
  };
  
  