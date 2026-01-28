import ClientPage from './ClientPage';

export async function generateStaticParams() {
    return [{ id: '1', discussionId: '1' }];
}

export default async function DiscussionPage({ params }: { params: Promise<{ discussionId: string }> }) {
    const { discussionId } = await params;
    return <ClientPage discussionId={discussionId} />;
}
