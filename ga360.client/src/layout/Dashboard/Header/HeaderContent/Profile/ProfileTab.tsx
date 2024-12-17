import { useState } from "react";
import { useNavigate } from "react-router";

// material-ui
import List from "@mui/material/List";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";

// assets
import EditOutlined from "@ant-design/icons/EditOutlined";
import ProfileOutlined from "@ant-design/icons/ProfileOutlined";
import LogoutOutlined from "@ant-design/icons/LogoutOutlined";
import UserOutlined from "@ant-design/icons/UserOutlined";
import WalletOutlined from "@ant-design/icons/WalletOutlined";

interface Props {
  handleLogout: () => void;
}

// ==============================|| HEADER PROFILE - PROFILE TAB ||============================== //

export default function ProfileTab({ handleLogout }: Props) {
  const navigate = useNavigate();
  const [selectedIndex, setSelectedIndex] = useState(0);

  const handleListItemClick = (
    event: React.MouseEvent<HTMLDivElement>,
    index: number,
    route: string = ""
  ) => {
    setSelectedIndex(index);

    if (route && route !== "") {
      navigate(route);
    }
  };

  // const handleLogoutClick = async () => {
  //   await fetch("/bff/logout", {
  //     headers: {
  //       "X-CSRF": "Dog"
  //     }
  //   });
  //   window.location.href = 'https://localhost:5173/bff/login'
  // };

  const handleLogoutClick = async () => {
    // // Clear cookies on the client side if necessary
    // document.cookie.split(";").forEach(cookie => {
    //   document.cookie = cookie.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date(0).toUTCString() + ";path=/");
    // });

    // const cookies = document.cookie.split(";");

    // for (let i = 0; i < cookies.length; i++) {
    //   const cookie = cookies[i];
    //   console.log("COOCKIE", cookie)
    //   const eqPos = cookie.indexOf("=");
    //   const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
    //   document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    // }

    // await fetch("/api/configuration/sessionout", {
    //   method: "POST",
    //   headers: {
    //     "Content-Type": "application/json",
    //     "X-CSRF": "Dog",
    //   },
    // });

    // Redirect to the login page
    window.location.href = "https://app-ga360authn-prod-uksouth.azurewebsites.net/Account/Logout";
  };

  return (
    <List
      component="nav"
      sx={{ p: 0, "& .MuiListItemIcon-root": { minWidth: 32 } }}
    >
      {/* <ListItemButton
        selected={selectedIndex === 0}
        onClick={(event: React.MouseEvent<HTMLDivElement>) =>
          handleListItemClick(event, 0, "/apps/profiles/user/personal")
        }
      >
        <ListItemIcon>
          <EditOutlined />
        </ListItemIcon>
        <ListItemText primary="Edit Profile" />
      </ListItemButton>
      <ListItemButton
        selected={selectedIndex === 1}
        onClick={(event: React.MouseEvent<HTMLDivElement>) =>
          handleListItemClick(event, 1, "/apps/profiles/account/basic")
        }
      >
        <ListItemIcon>
          <UserOutlined />
        </ListItemIcon>
        <ListItemText primary="View Profile" />
      </ListItemButton>

      <ListItemButton
        selected={selectedIndex === 3}
        onClick={(event: React.MouseEvent<HTMLDivElement>) =>
          handleListItemClick(event, 3, "apps/profiles/account/personal")
        }
      >
        <ListItemIcon>
          <ProfileOutlined />
        </ListItemIcon>
        <ListItemText primary="Social Profile" />
      </ListItemButton>
      <ListItemButton
        selected={selectedIndex === 4}
        onClick={(event: React.MouseEvent<HTMLDivElement>) =>
          handleListItemClick(event, 4, "/apps/invoice/details/1")
        }
      >
        <ListItemIcon>
          <WalletOutlined />
        </ListItemIcon>
        <ListItemText primary="Billing" />
      </ListItemButton> */}
      {/* <ListItemButton selected={selectedIndex === 2} onClick={handleLogout}> */}
      {/* <ListItemButton
        selected={selectedIndex === 2}
        onClick={handleLogoutClick}
      >
        <ListItemIcon>
          <LogoutOutlined />
        </ListItemIcon>
        <ListItemText primary="Logout" />
      </ListItemButton> */}
    </List>
  );
}
