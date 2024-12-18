import { Grid, Paper, Typography } from "@mui/material";
import Box from "@mui/system/Box";
import { useEffect, useState } from "react";
import { styled } from '@mui/material/styles';
import { CustomerList } from "../../types/customer";

interface DataObject {
    type: string;
    value: string | number;
}
const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
  }));
export default function ClaimsDashboard() {
    const [claims, setClaims] = useState<DataObject[]>();
    const [user, setUser] = useState<string | number>();
    const [logoutUrl, setLogoutUrl] = useState<string>();
    const [authNEndpoint, setAuthNEndpoint] = useState<string>();
    const [allowedMenu, setAllowedMenu] = useState<string>();
    const [allowedCustomers, setAllowedCustomers] = useState<CustomerList[]>();

    useEffect(() => {
        fetchUserSessionInfo();
    }, []);

    useEffect(() => {
        const fetchData = async () => {
          try {
              const result = await fetchAllowedLandingMenu();
              const customers = await fetchCustomerList();
              setAllowedMenu(result);

              setAllowedCustomers(customers);
          } catch (error) {
            console.error('Error fetching data:', error);
          }
        };
    
        const fetchAuthzData = async () => {
            try {
             const result = await fetchAuthNdLandingMenu();
             setAuthNEndpoint(result);
            } catch (error) {
              console.error('Error fetching data:', error);
            }
        };


        fetchData();
        fetchAuthzData();
      }, []);

    const loginscreen = claims === undefined ?
        <a className="text-dark nav-link" href="/bff/login">
            Login
        </a>
        : <span>
            <Grid>
            <Grid item xs={6}>
            <a className="text-dark nav-link" href={"https://localhost:5173"+logoutUrl}>
                Log out {"https://localhost:5173"+logoutUrl}
            </a>
            </Grid>
            <Grid>
                {user}
            </Grid>
            </Grid>
        </span>
        

    const claimsresults = claims === undefined
        ? <p><em>If claims have not been loaded, please login.</em></p>
        : <>
            <table>
                <thead>
                    <tr>
                        <th>Type</th>
                        <th>Value</th>
                    </tr>
                </thead>
                <tbody>
                    {claims.map((claim) => (
                        <tr key={claim.type}>
                            <td>{claim.type}</td>
                            <td>{claim.value}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>

    return (
        <>
        <Box sx={{ flexGrow: 1 }}>
        <Grid container spacing={2}>
            <Grid item xs={8}>
                <Item>{claimsresults}</Item>
            </Grid>
            <Grid item xs={4}>
                <Item>  {loginscreen}</Item>
            </Grid>
            <Grid item xs={4}>
                <Item>Annonymous Menu</Item>
            </Grid>
            <Grid item xs={8}>
                        <Item>{allowedMenu  ? allowedMenu:"Loading..."}</Item>
            </Grid>
            <Grid item xs={4}>
                <Item>Authorize Menu</Item>
            </Grid>
            <Grid item xs={8}>
                <Item>{authNEndpoint}</Item>
            </Grid>
            <Grid item xs={4}>
               <Item>Customers Menu</Item>
            </Grid>
            <Grid item xs={8}>
                        <Item>{allowedCustomers?.map(x => {
                            return <Grid>{x.name}</Grid>
                        })}</Item>
            </Grid>
        </Grid>
    </Box>
        </>
    );

    async function fetchUserSessionInfo() {
        const response = await fetch("/bff/user", {
            headers: {
                "X-CSRF": "Dog",
            },
        });
        if (response.ok) {

            const data: DataObject[] = await response.json();
            const name: (string | number | undefined) = data.find((x: { type: string; }) => x.type === 'name')?.value;
            const id: (string | number | undefined) = data.find((x: { type: string; }) => x.type === 'id')?.value;
            const email: (string | number | undefined) = data.find((x: { type: string; }) => x.type === 'email')?.value;
            const logouturl: (string | number | undefined) = data.find((x: { type: string; }) => x.type === 'bff:logout_url')?.value as string;
            console.log(logouturl)
            const user = {
                name: name,
                id: id,
                email: email
            };

            setClaims(data);
            setUser(email);
            setLogoutUrl(logouturl);
        }
    }

    async function fetchAllowedLandingMenu() {
        const response = await fetch("/menu/landing", {
            headers: {
                "X-CSRF": "Dog",
            },
        });

        const result = await response.text();

        return result;
    }

    async function fetchAuthNdLandingMenu() {
        const response = await fetch("/menu/authnlanding", {
            headers: {
                "X-CSRF": "Dog",
            },
        });

        const result = await response.text();

        return result;
    }

    async function fetchCustomerList() {
        const response = await fetch("api/customer/list", {
            headers: {
                "X-CSRF": "Dog",
            },
        });

        const result = await response.json();

        return result;
    }
}