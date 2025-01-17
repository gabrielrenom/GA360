import { useEffect, useState } from 'react';
import { useLocation, Link, Outlet, useParams } from 'react-router-dom';

// material-ui
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import Tabs from '@mui/material/Tabs';

// project import
import MainCard from 'components/MainCard';

// assets
import ContainerOutlined from '@ant-design/icons/ContainerOutlined';
import FileTextOutlined from '@ant-design/icons/FileTextOutlined';
import LockOutlined from '@ant-design/icons/LockOutlined';
import UserOutlined from '@ant-design/icons/UserOutlined';

// ==============================|| PROFILE - ACCOUNT ||============================== //

export default function CRUDCandidateProfile() {
  const { pathname } = useLocation();
  const { id } = useParams();

  let selectedTab = 0;
  let breadcrumbTitle = '';
  let breadcrumbHeading = '';

  switch (pathname) {
    case `/apps/profiles/crudcandidate/profile/${id}`:
      breadcrumbTitle = 'Profile';
      breadcrumbHeading = 'Profile';
      selectedTab = 0;
      break;
    case `/apps/profiles/crudcandidate/qualifications/${id}`:
      breadcrumbTitle = 'Qualifications';
      breadcrumbHeading = 'Qualifications';
      selectedTab = 1;
      break;
    case `/apps/profiles/crudcandidate/courses/${id}`:
      breadcrumbTitle = 'Courses';
      breadcrumbHeading = 'Courses';
      selectedTab = 2;
      break;
    case `/apps/profiles/crudcandidate/documents/${id}`:
      breadcrumbTitle = 'Documents';
      breadcrumbHeading = 'Documents';
      selectedTab = 3;
      break;
    default:
      breadcrumbTitle = 'Profile';
      breadcrumbHeading = 'Profile';
      selectedTab = 0;
  }

  const [value, setValue] = useState(selectedTab);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  useEffect(() => {
    if (pathname.includes(`/apps/profiles/crudcandidate/profile/${id}`)) {
      setValue(0);
    } else if (pathname.includes(`/apps/profiles/crudcandidate/qualifications/${id}`)) {
      setValue(1);
    } else if (pathname.includes(`/apps/profiles/crudcandidate/courses/${id}`)) {
      setValue(2);
    } else if (pathname.includes(`/apps/profiles/crudcandidate/documents/${id}`)) {
      setValue(3);
    }
  }, [pathname, id]);

  return (
    <>
      <MainCard border={false} boxShadow>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', width: '100%' }}>
          <Tabs value={value} onChange={handleChange} variant="scrollable" scrollButtons="auto" aria-label="account profile tab">
            <Tab
              label="Profile"
              component={Link}
              to={`/apps/profiles/crudcandidate/profile/${id}`}
              icon={<UserOutlined />}
              iconPosition="start"
            />
            <Tab
              label="Qualifications"
              component={Link}
              to={`/apps/profiles/crudcandidate/qualifications/${id}`}
              icon={<FileTextOutlined />}
              iconPosition="start"
            />
            <Tab
              label="Courses"
              component={Link}
              to={`/apps/profiles/crudcandidate/courses/${id}`}
              icon={<ContainerOutlined />}
              iconPosition="start"
            />
            <Tab
              label="Documents"
              component={Link}
              to={`/apps/profiles/crudcandidate/documents/${id}`}
              icon={<LockOutlined />}
              iconPosition="start"
            />
          </Tabs>
        </Box>
        <Box sx={{ mt: 2.5 }}>
          <Outlet />
        </Box>
      </MainCard>
    </>
  );
}
