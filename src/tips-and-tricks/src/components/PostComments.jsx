export default function PostComments({ comments }) {
	return (
		<div className='mt-3'>
			<h4 className='d-flex gap-2'>
				<span>Bình luận</span>
				<span>({comments.length})</span>
			</h4>
			{comments.map((comment) => (
				<div
					key={comment.id}
					className='mb-3 p-4 bg-secondary bg-opacity-10 border border-1 border-gray rounded'
				>
					<div>
						<span className='fs-6 fw-bold me-2'>
							{comment.name}
						</span>
						<span className='text-secondary fst-italic'>
							{new Date(comment.postedDate).toLocaleDateString(
								'vi-VN',
							)}
						</span>
					</div>
					<p className='mb-0'>{comment.description}</p>
				</div>
			))}
		</div>
	);
}
