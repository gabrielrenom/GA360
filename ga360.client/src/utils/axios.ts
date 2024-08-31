import axios, { AxiosRequestConfig } from 'axios';

//const axiosServices = axios.create({ baseURL: import.meta.env.VITE_APP_API_URL || 'http://localhost:3010/' });
const axiosServices = axios.create();

// ==============================|| AXIOS - FOR MOCK SERVICES ||============================== //

axiosServices.interceptors.request.use(
  async (config) => {

    const accessToken = localStorage.getItem('serviceToken');

    if (accessToken) {
      config.headers['Authorization'] = `Bearer ${accessToken}`;
      console.log(config.headers['Authorization'])
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosServices.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response.status === 401 && !window.location.href.includes('/login')) {
      window.location.pathname = '/maintenance/500';
    }

    return Promise.reject((error.response && error.response.data) || 'Wrong Services');
  }
);

export default axiosServices;

export const fetcher = async (args: string | [string, AxiosRequestConfig]) => {
  const [url, config] = Array.isArray(args) ? args : [args];

  const res = await axiosServices.get(url, { ...config });

  console.log(res.data)
  return res.data;
};

//export const fetcher = async (args: string | [string, RequestInit]): Promise<any> => {
//    const [url, config] = Array.isArray(args) ? args : [args];

//    try {
//        const response = await fetch(url, { ...config });
//        if (!response.ok) {
//            throw new Error(`HTTP error! status: ${response.status}`);
//        }
//        console.log(response);
//        const data = await response.json();
//        return data;
//    } catch (error) {
//        console.error('Error fetching data:', error);
//        throw error;
//    }
//};


export const fetcherPost = async (args: string | [string, AxiosRequestConfig]) => {
  const [url, config] = Array.isArray(args) ? args : [args];

  const res = await axiosServices.post(url, { ...config });

  return res.data;
};
