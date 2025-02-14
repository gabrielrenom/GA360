import { useContext, useEffect } from 'react';
import { Outlet } from 'react-router-dom';

// material-ui
import { Theme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Toolbar from '@mui/material/Toolbar';

// project import
import Drawer from './Drawer';
import Header from './Header';
import Footer from './Footer';
import HorizontalBar from './Drawer/HorizontalBar';
import Loader from 'components/Loader';
import Breadcrumbs from 'components/@extended/Breadcrumbs';
import AddCustomer from 'sections/apps/customer/AddCustomer';
import AuthGuard from 'utils/route-guard/AuthGuard';

import { MenuOrientation } from 'config';
import useConfig from 'hooks/useConfig';
import { handlerDrawerOpen, useGetMenuMaster } from 'api/menu';
import DynamicTableCourse from 'sections/apps/course/DynamicTableCourse';
import DynamicTableQualification from 'sections/apps/course/DynamicTableQualifications';
import DynamicTableTrainingCentre from 'sections/apps/course/DynamicTableTrainingCentre';
import DynamicTableCustomersWithCourseQualificationRecords from 'sections/apps/course/DynamicTableCustomersWithCourseQualificationRecords';
import DuendeContext from 'contexts/DuendeContext';
import DynamicTableCustomerWithCourseQualificationRecords from 'sections/apps/course/DynamicTableCustomerWithCourseQualificationRecords';

// ==============================|| MAIN LAYOUT ||============================== //

export default function DashboardLayout() {
  const { menuMasterLoading } = useGetMenuMaster();
  const downXL = useMediaQuery((theme: Theme) => theme.breakpoints.down('xl'));
  const downLG = useMediaQuery((theme: Theme) => theme.breakpoints.down('lg'));

  const { container, miniDrawer, menuOrientation } = useConfig();
  const { user, isLoggedIn } = useContext(DuendeContext);

  const isHorizontal = menuOrientation === MenuOrientation.HORIZONTAL && !downLG;

  // set media wise responsive drawer
  useEffect(() => {
    if (!miniDrawer) {
      handlerDrawerOpen(!downXL);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [downXL]);

  if (menuMasterLoading) return <Loader />;

  return (
    <AuthGuard>
      <Box sx={{ display: 'flex', width: '100%' }}>
        <Header />
        {user && user.role !=="Candidate"?( 
        !isHorizontal ? <Drawer /> : <HorizontalBar />):<></>
        }
        {/* <DummyComponent></DummyComponent> */}
        <Box component="main" sx={{ width: 'calc(100% - 260px)', flexGrow: 1, p: { xs: 2, sm: 3 } }}>
          <></>
          {user && user.role !=="Candidate"?
            <Toolbar sx={{ mt: isHorizontal ? 8 : 'inherit' }} /> :<></>}
          <Container
            maxWidth={container ? 'xl' : false}
            sx={{
              ...(container && { px: { xs: 0, sm: 2 } }),
              position: 'relative',
              minHeight: 'calc(100vh - 110px)',
              display: 'flex',
              flexDirection: 'column'
            }}
          >
            <Breadcrumbs />
            {/* <DynamicTableCustomerWithCourseQualificationRecords></DynamicTableCustomerWithCourseQualificationRecords> */}
            <Outlet />
       

            <Footer />
          </Container>
        </Box>
        {/* <AddCustomer /> */}
      </Box>
    </AuthGuard>
  );
}


