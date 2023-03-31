import { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';

import { API_URL } from '../../utils/constants';
import { getPostBySlug } from '../../services/posts';

import TagList from '../../components/TagList';
import NotFound from '../NotFound';

export default function Post() {
	// Component's variables
	const params = useParams();

	// Component's states
	const [post, setPost] = useState({});

	useEffect(() => {
		fetchPost();

		async function fetchPost() {
			const data = await getPostBySlug(params.slug);
			if (data) setPost(data);
			else setPost({});
		}
	}, [params]);

	return (
		<>
			{post.id ? (
				<div className='p-4'>
					<h1>{post.title}</h1>
					<div className='d-flex align-items-center justify-content-between my-2'>
						<div>
							<span>Chủ đề:</span>
							<Link
								to={`/blog/category/${post.category.urlSlug}`}
								className='ms-2 text-decoration-none'
							>
								{post.category.name}
							</Link>
						</div>
						<div>
							<TagList tags={post.tags} />
						</div>
					</div>
					<div className='d-flex justify-content-between'>
						<div className='d-flex gap-1'>
							<span>Đăng ngày:</span>
							<span>
								{new Date(post.postedDate).toLocaleDateString(
									'vi-VN',
								)}
							</span>
							<span>bởi:</span>
							<Link
								to={`/blog/author/${post.author.urlSlug}`}
								className='text-decoration-none'
							>
								{post.author.fullName}
							</Link>
						</div>
						<div>
							<span className='me-1'>{post.viewCount}</span>
							<span>lượt xem</span>
						</div>
					</div>
					<div className='mt-3'>
						{post.imageUrl && (
							<img
								src={`${API_URL}/${post.imageUrl}`}
								className='w-100 rounded shadow'
								alt='thumbnail'
							/>
						)}
						<p className='mt-3'>{post.description}</p>
					</div>
				</div>
			) : (
				<NotFound />
			)}
		</>
	);
}
