import { useEffect, useState } from 'react';
import { useLocation, Link, Outlet } from 'react-router-dom';

// material-ui
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import Tabs from '@mui/material/Tabs';

// project import
import MainCard from 'components/MainCard';
import Breadcrumbs from 'components/@extended/Breadcrumbs';
import { APP_DEFAULT_PATH } from 'config';

// assets
import ContainerOutlined from '@ant-design/icons/ContainerOutlined';
import FileTextOutlined from '@ant-design/icons/FileTextOutlined';
import LockOutlined from '@ant-design/icons/LockOutlined';
import SettingOutlined from '@ant-design/icons/SettingOutlined';
import TeamOutlined from '@ant-design/icons/TeamOutlined';
import UserOutlined from '@ant-design/icons/UserOutlined';

// ==============================|| PROFILE - ACCOUNT ||============================== //

export default function TrainingCentreProfile() {
  const { pathname } = useLocation();

  let selectedTab = 0;
  let breadcrumbTitle = '';
  let breadcrumbHeading = '';
  switch (pathname) {
    case '/apps/profiles/trainingcentre/profile':
      breadcrumbTitle = 'Profile';
      breadcrumbHeading = 'Profile';
      selectedTab = 0;
      break;
    case '/apps/profiles/trainingcentre/qualifications':
      breadcrumbTitle = 'Qualifications';
      breadcrumbHeading = 'Qualifications';
      selectedTab = 1;
      break;
    case '/apps/profiles/trainingcentre/courses':
      breadcrumbTitle = 'Courses';
      breadcrumbHeading = 'Courses';
      selectedTab = 2;
      break;
    case '/apps/profiles/trainingcentre/documents':
      breadcrumbTitle = 'Documents';
      breadcrumbHeading = 'Documents';
      selectedTab = 3;
      break;
    // case '/apps/profiles/trainingcentre/certificates':
    //   breadcrumbTitle = 'Cards & Certs';
    //   breadcrumbHeading = 'Cards & Certs';
    //   selectedTab = 4;
    //   break;
    default:
      breadcrumbTitle = 'Profile';
      breadcrumbHeading = 'Profile';
      selectedTab = 0;
  }

  const [value, setValue] = useState(selectedTab);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  let breadcrumbLinks = [
    { title: 'Home', to: APP_DEFAULT_PATH },
    { title: 'Account Profile', to: '/apps/profiles/trainingcentre/profile' },
    { title: breadcrumbTitle }
  ];
  if (selectedTab === 0) {
    breadcrumbLinks = [{ title: 'Home', to: APP_DEFAULT_PATH }, { title: 'Training Centre Profile' }];
  }

  useEffect(() => {
    if (pathname === '/apps/profiles/trainingcentre/profile') {
      setValue(0);
    }
  }, [pathname]);

  return (
    <>
      <Breadcrumbs custom heading={breadcrumbHeading} links={breadcrumbLinks} />
      <MainCard border={false} boxShadow>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', width: '100%' }}>
          <Tabs value={value} onChange={handleChange} variant="scrollable" scrollButtons="auto" aria-label="account profile tab">
            {/* TAB PROFILE */}
            <Tab label="Profile" component={Link} to="/apps/profiles/trainingcentre/profile" icon={<UserOutlined />} iconPosition="start" />
            
            {/* TAB QUALIFICATIONS */}
            <Tab label="Qualifications" component={Link} to="/apps/profiles/trainingcentre/qualifications" icon={<FileTextOutlined />} iconPosition="start" />
            
            {/* TAB QUALIFICATIONS */}
            <Tab
              label="Courses"
              component={Link}
              to="/apps/profiles/trainingcentre/courses"
              icon={<ContainerOutlined />}
              iconPosition="start"
            />

            {/* TAB QUALIFICATIONS */}
            <Tab
              label="Documents"
              component={Link}
              to="/apps/profiles/trainingcentre/documents"
              icon={<LockOutlined />}
              iconPosition="start"
            />

            {/* TAB CARDS & CERTS */}
            {/* <Tab label="Cards & Certs" component={Link} to="/apps/profiles/trainingcentre/profile" icon={<TeamOutlined />} iconPosition="start" /> */}
          </Tabs>
        </Box>
        <Box sx={{ mt: 2.5 }}>
          <Outlet />
        </Box>
      </MainCard>
    </>
  );
}
