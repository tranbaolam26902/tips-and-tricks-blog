import { Outlet } from 'react-router-dom';

import Navigation from '../../components/admin/Navigation';

export default function Layout() {
	return (
		<>
			<Navigation />
			<div className='container-fluid py-3'>
				<Outlet />
			</div>
		</>
	);
}
