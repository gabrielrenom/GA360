import React, { createContext, useEffect, useReducer } from 'react';

// third-party
import { Chance } from 'chance';
import { jwtDecode } from 'jwt-decode';

// reducer - state management
import { LOGIN, LOGOUT } from 'contexts/auth-reducer/actions';
import authReducer from 'contexts/auth-reducer/auth';

// project import
import Loader from 'components/Loader';
import axios from 'utils/axios';
import { AuthProps } from 'types/auth';
import { User } from 'types/customer';
import { getUser } from "api/customer";

const chance = new Chance();

// constant
const initialState: AuthProps = {
    isLoggedIn: false,
    isInitialized: false,
    user: null
};

const verifyToken = (serviceToken) => {
    if (!serviceToken) {
        return false;
    }
    const decoded = jwtDecode(serviceToken);
    return decoded.exp > Date.now() / 1000;
};

const setSession = (serviceToken) => {
    if (serviceToken) {
        localStorage.setItem('serviceToken', serviceToken);
        axios.defaults.headers.common.Authorization = `Bearer ${serviceToken}`;
    } else {
        localStorage.removeItem('serviceToken');
        delete axios.defaults.headers.common.Authorization;
    }
};

// ==============================|| JWT CONTEXT & PROVIDER ||============================== //

const DuendeContext = createContext(null);

export  const DuendeProvider = ({ children }) => {
    const [state, dispatch] = useReducer(authReducer, initialState);

    useEffect(() => {
        const init = async () => {
            
            try {
                const response = await fetch("/bff/user", {
                    headers: {
                        "X-CSRF": "Dog",
                    },
                });
                if (response.ok) {
                    const userResult = await response.json();
                    const name = userResult.find((x: { type: string; }) => x.type === 'name').value;
                    const id = userResult.find((x: { type: string; }) => x.type === 'sid').value;
                    const email = userResult.find((x: { type: string; }) => x.type === 'email').value;
                    
                    const userDetails: User = await getUser(); 
                    
                    const user = {
                        name: name,
                        id: id,
                        email: email,
                        roleId: userDetails.roleId,
                        role: userDetails.role,
                        firstName: userDetails.firstName,
                        lastName: userDetails.lastName
                    };
                    dispatch({
                        type: LOGIN,
                        payload: {
                            isLoggedIn: true,
                            user
                        }
                    });
                } else {
                    dispatch({
                        type: LOGOUT
                    });
                }
            } catch (err) {
                console.error(err);
                dispatch({
                    type: LOGOUT
                });
            }
        };

        init();
    }, []);

    const login = async (email, password) => {
        const response = await axios.post('/api/account/login', { email, password });
        const { serviceToken, user } = response.data;
        setSession(serviceToken);
        dispatch({
            type: LOGIN,
            payload: {
                isLoggedIn: true,
                user
            }
        });
    };

    const register = async (email, password, firstName, lastName) => {
        const id = chance.bb_pin();
        const response = await axios.post('/api/account/register', {
            id,
            email,
            password,
            firstName,
            lastName
        });
        let users = response.data;

        if (window.localStorage.getItem('users') !== undefined && window.localStorage.getItem('users') !== null) {
            const localUsers = window.localStorage.getItem('users');
            users = [
                ...JSON.parse(localUsers),
                {
                    id,
                    email,
                    password,
                    name: `${firstName} ${lastName}`
                }
            ];
        }

        window.localStorage.setItem('users', JSON.stringify(users));
    };

    const logout = async () => {
        await fetch("/bff/logout", {
            headers: {
                "X-CSRF": "Dog",
            },
        });
        dispatch({ type: LOGOUT });
    };
    const resetPassword = async (email) => {
        console.log('email - ', email);
    };

    const updateProfile = () => { };

    if (state.isInitialized !== undefined && !state.isInitialized) {
        return <Loader />;
    }

    return (
        <DuendeContext.Provider value={{ ...state, login, logout, register, resetPassword, updateProfile }}>
            {children}
        </DuendeContext.Provider>
    );
};

export default DuendeContext;
