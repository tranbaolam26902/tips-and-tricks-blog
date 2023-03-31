export default function About() {
	return (
		<div className='p-5'>
			<h2 className='mb-5 text-uppercase text-center'>Giới thiệu</h2>
			<iframe
				className='w-100'
				height='640'
				src='https://www.youtube.com/embed/dBDBq7FjogM'
				title='Zoolander meme BlvckVines x Kenjumboy.'
				allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share'
				allowFullScreen
			></iframe>
			<div className='mt-5'>
				<div className='d-flex justify-content-between row gx-4'>
					<img
						src='/assets/images/about-01.jpg'
						className='w-50'
						alt='about-image'
					/>
					<img
						src='/assets/images/about-02.jpg'
						className='w-50'
						alt='about-image'
					/>
				</div>
				<div className='d-flex justify-content-between row gx-4 mt-4'>
					<img
						src='/assets/images/about-03.jpg'
						className='w-50'
						alt='about-image'
					/>
					<img
						src='/assets/images/about-04.jpg'
						className='w-50'
						alt='about-image'
					/>
				</div>
			</div>
		</div>
	);
}
