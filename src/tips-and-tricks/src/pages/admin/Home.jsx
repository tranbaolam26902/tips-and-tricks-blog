// Libraries
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

// App's features
import { getDashboardData } from '../../services/dashboard';

export default function Home() {
	const [dashboard, setDashboard] = useState();

	useEffect(() => {
		fetchData();

		async function fetchData() {
			const data = await getDashboardData();

			if (data) setDashboard(data);
			else setDashboard({});
		}
	}, []);

	return (
		<div className='mx-4'>
			<h1 className='pt-3 mb-5 text-uppercase'>Tổng quan</h1>
			{dashboard && (
				<>
					<div className='row'>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-primary'>
								<span className='fs-3'>Tổng số bài viết</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalPosts}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-secondary'>
								<span className='fs-3'>
									Số bài viết chưa xuất bản
								</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalUnpublishedPosts}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-warning'>
								<span className='fs-3'>Tổng số chủ đề</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalCategories}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-danger'>
								<span className='fs-3'>Tổng số tác giả</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalAuthors}
								</span>
							</div>
						</div>
					</div>
					<div className='row'>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-success'>
								<span className='fs-3'>
									Số bình luận chờ phê duyệt
								</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalNotApprovedComments}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-info'>
								<span className='fs-3'>
									Tổng số người theo dõi
								</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalSubscribers}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3'>
							<div className='d-flex flex-column p-3 shadow border border-5 border-top-0 border-bottom-0 border-end-0 border-dark'>
								<span className='fs-3'>
									Số người theo dõi hôm nay
								</span>
								<span className='d-flex align-items-center flex-1 fs-1 fw-bold'>
									{dashboard.totalSubscribersToday || '0'}
								</span>
							</div>
						</div>
						<div className='col-lg-3 col-md-6 col-sm-12 mb-3 d-flex align-items-center justify-content-center'>
							<Link to='/' className='text-primary fs-1'>
								Trang chủ
							</Link>
						</div>
					</div>
				</>
			)}
		</div>
	);
}
