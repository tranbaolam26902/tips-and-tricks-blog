export default function Subscribe() {
	return (
		<div
			className='mx-auto mt-5 p-5 rounded shadow'
			style={{ maxWidth: 576 }}
		>
			<h5 className='text-uppercase'>Đăng ký nhận thông báo</h5>
			<p>Nhận thông báo về tin tức, bài viết mới thông qua email.</p>
			<form>
				<div className='d-flex'>
					<input
						type='email'
						name='email'
						className='flex-fill px-2 rounded border border-1 border secondary'
						placeholder='Email của bạn'
						required
					/>
					<button
						type='submit'
						className='btn btn-primary ms-3 px-4 py-2'
					>
						Đăng ký
					</button>
				</div>
				<div className='mt-3 text-danger'></div>
			</form>
		</div>
	);
}
